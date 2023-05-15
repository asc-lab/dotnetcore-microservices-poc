FROM node:10.24.0-alpine3.11 AS builder
WORKDIR /app

COPY package*.json ./
RUN apk add --no-cache ffmpeg opus pixman cairo pango giflib ca-certificates \
    && apk add --no-cache --virtual .build-deps python g++ make gcc .build-deps curl git pixman-dev cairo-dev pangomm-dev libjpeg-turbo-dev giflib-dev \
    && npm install

COPY . .
ENV VUE_APP_CHAT_URL=http://localhost:4099
RUN npm run build

FROM nginx:stable-perl
RUN rm /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx/conf.d
COPY --from=builder /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
