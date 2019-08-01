FROM node:lts-alpine AS builder
WORKDIR /app

COPY package*.json ./

RUN npm install
COPY . .
RUN npm run build

FROM nginx:1.12-alpine

ENV \
    GATEWAY_URL=${GATEWAY_URL:-http://localhost:8081} \
    AUTH_URL=${AUTH_URL:-http://localhost:8090}

COPY nginx-app.conf /etc/nginx/conf.d/default.tmpl
COPY --from=builder /app/dist /usr/share/nginx/html
COPY entrypoint.sh .

ENTRYPOINT ["/entrypoint.sh"]
