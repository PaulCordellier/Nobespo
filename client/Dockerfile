# build stage
FROM node:lts-alpine AS build-stage
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# ne peut pas configurer https ici : https est géré avec les infos qu'il y a dans le dossier Serveur, qui est pas dans le container. Voir vite.config.js

# production stage
FROM nginx:stable-alpine AS production-stage
COPY --from=build-stage /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
RUN wget https://raw.githubusercontent.com/nginx/nginx/master/conf/mime.types -O /etc/nginx/mime.types
RUN chmod 644 /etc/nginx/mime.types
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]

# Commandes utilisées pour lancer le projet :
# docker build . -t nobespo-vue-app
# docker run -it -p 8221:80 --rm --name dockerize-nobespo-vue-app nobespo-vue-app