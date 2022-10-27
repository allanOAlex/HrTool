FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY app/publish/wwwroot .
COPY GSG.WebApp/NGINX/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80


