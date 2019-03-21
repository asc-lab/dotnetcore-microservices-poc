<template>
    <div class="form-container">
        <h2>Fill information about Policy Holder</h2>
        <b-form @submit="createPolicy">
            <b-form-group id="firstNameGroup"
                          label="First name:"
                          label-for="firstName">
                <b-form-input id="firstName"
                              type="text"
                              v-model="policyHolder.firstName"
                              required
                              placeholder="Enter first name">
                </b-form-input>
            </b-form-group>

            <b-form-group id="lastNameGroup"
                          label="Last name:"
                          label-for="lastName">
                <b-form-input id="lastName"
                              type="text"
                              v-model="policyHolder.lastName"
                              required
                              placeholder="Enter last name">
                </b-form-input>
            </b-form-group>

            <b-form-group id="taxIdGroup"
                          label="Tax id:"
                          label-for="taxId">
                <b-form-input id="taxId"
                              type="text"
                              v-model="policyHolder.taxId"
                              required
                              placeholder="Enter tax id">
                </b-form-input>
            </b-form-group>

            <b-form-group id="countryGroup"
                          label="Country:"
                          label-for="country">

                <select required class="form-control" v-model="policyAddress.country">
                    <option v-for="c in countries" v-bind:key="c" >
                        {{ c }}
                    </option>
                </select>
            </b-form-group>

            <b-form-group id="zipCodeGroup"
                          label="Zip Code:"
                          label-for="zipCode">
                <b-form-input id="zipCode"
                              type="text"
                              v-model="policyAddress.zipCode"
                              required
                              placeholder="Enter zip code">
                </b-form-input>
            </b-form-group>

            <b-form-group id="cityGroup"
                          label="City:"
                          label-for="city">
                <b-form-input id="city"
                              type="text"
                              v-model="policyAddress.city"
                              required
                              placeholder="Enter city">
                </b-form-input>
            </b-form-group>

            <b-form-group id="streetGroup"
                          label="Street:"
                          label-for="street">
                <b-form-input id="street"
                              type="text"
                              v-model="policyAddress.street"
                              required
                              placeholder="Enter street">
                </b-form-input>
            </b-form-group>

            <b-button type="submit" variant="primary">Confirm</b-button>
        </b-form>

    </div>
</template>

<script>
    import {HTTP} from "./http/ApiClient";

    export default {
        name: "PolicyCreate",
        props: {
            offerNumber: String
        },
        data() {
            return {
                policyHolder: {
                    firstName: '',
                    lastName: '',
                    taxId: ''
                },
                policyAddress: {
                    country: 'Poland',
                    zipCode: '',
                    city: '',
                    street: ''
                },
                countries: [
                    'Poland',
                    'France',
                    'Germany'
                ]
            }
        },
        methods: {
            createPolicy: function (evt) {
                evt.preventDefault();

                const request = {
                    offerNumber: this.offerNumber,
                    policyHolder: this.policyHolder,
                    policyHolderAddress: this.policyAddress
                };

                HTTP.post('policies', request).then(response => {
                    this.$router.push({name: 'policyDetails', params: {policyNumber: response.data.policyNumber}});
                })
            }
        }
    }
</script>

<style scoped>
    .form-container {
        width: 40%;
        margin: 0 auto;
    }

    h2 {
        margin-bottom: 40px;
    }
</style>