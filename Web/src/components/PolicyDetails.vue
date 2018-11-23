<template>
    <div>
        <h3>Policy details: {{ policy.number }}</h3>
        <div>
            <div class="row">
                <span><strong>From:</strong> {{ policy.dateFrom }}</span>
            </div>
            <div class="row">
                <span><strong>To:</strong> {{ policy.dateTo }}</span>
            </div>
            <div class="row">
                <span><strong>Policy holder:</strong> {{ policy.policyHolder }}</span>
            </div>
            <div class="row">
                <span><strong>Total premium:</strong> {{ policy.totalPremium }} EUR</span>
            </div>
            <div class="row">
                <span><strong>Product code:</strong> {{ policy.productCode }}</span>
            </div>
            <div class="row">
                <span><strong>Account number:</strong> {{ policy.accountNumber }}</span>
            </div>
            <div class="row">
                <span><strong>Covers:</strong> {{ policy.covers | join }}</span>
            </div>
            <div class="row">
                <button type="submit"
                        class="btn btn-primary"
                        v-on:click.stop.prevent="documents">Documents
                </button>
            </div>
        </div>
    </div>
</template>

<script>
    import {HTTP} from "./http/ApiClient";

    export default {
        name: "PolicyDetails",
        props: {
            policyNumber: String
        },
        data() {
            return {
                policy: {},
                documentsList: []
            }
        },
        created: function () {
            HTTP.get("policies/" + this.policyNumber).then(response => {
                this.policy = response.data.policy;
            })
        },
        filters: {
            join: function (value) {
                if (!value)
                    return '';

                return value.join(', ');
            }
        },
        methods: {
            documents: function () {
                HTTP.get("documents/" + this.policyNumber).then(response => {
                    this.documentsList = response.data.documents;
                    this.documentsList.forEach((doc) => {
                        const data = doc.content;
                        const filename = 'Policy-' + this.policyNumber + '.pdf';
                        const blob = b64toBlob(data, '', 8),
                            e = document.createEvent('MouseEvents'),
                            a = document.createElement('a');
                        a.download = filename;
                        a.href = window.URL.createObjectURL(blob);
                        a.dataset.downloadurl = ['text/json', a.download, a.href].join(':');
                        e.initMouseEvent('click', true, false, window,
                            0, 0, 0, 0, 0, false, false, false, false, 0, null);
                        a.dispatchEvent(e);
                    })
                });

                function b64toBlob(b64Data, contentType, sliceSize) {
                    contentType = contentType || '';
                    sliceSize = sliceSize || 512;

                    const byteCharacters = atob(b64Data);
                    const byteArrays = [];

                    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
                        const slice = byteCharacters.slice(offset, offset + sliceSize);

                        const byteNumbers = new Array(slice.length);
                        for (let i = 0; i < slice.length; i++) {
                            byteNumbers[i] = slice.charCodeAt(i);
                        }

                        const byteArray = new Uint8Array(byteNumbers);

                        byteArrays.push(byteArray);
                    }

                    return new Blob(byteArrays, {type: contentType});
                }
            }
        }
    }
</script>

<style scoped>
</style>