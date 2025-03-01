# 1. Folosim un container Node pentru a construi aplicația Angular
FROM node:18 AS build
WORKDIR /app

# 2. Copiem package.json și package-lock.json și instalăm dependințele
COPY package.json package-lock.json ./
RUN npm install

# 3. Copiem restul fișierelor și construim aplicația
COPY . .
RUN npm run build --prod

# 4. Folosim Nginx pentru a servi aplicația Angular
FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# 5. Copiem fișierele generate din build
COPY --from=build /app/dist/frontend/browser /usr/share/nginx/html

# 6. Copiem configurarea Nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

# 7. Expunem portul și rulăm serverul
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
