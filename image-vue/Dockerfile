FROM node:15 as build
WORKDIR /app
COPY package.json package-lock.json ./
RUN ["npm", "ci"]
COPY . ./
RUN ["npm", "run", "build"]

# final image
FROM nginx:1.19
EXPOSE 80
COPY --from=build app/dist /usr/share/nginx/html