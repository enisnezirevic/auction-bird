﻿FROM node:21.6.0-alpine AS development
ENV NODE_ENV development

WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
EXPOSE 3000

CMD ["npm", "run", "dev"]