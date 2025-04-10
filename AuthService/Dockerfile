FROM node:20
WORKDIR /app
COPY package*.json ./
ENV USE_DB=true
RUN npm install
COPY . .
EXPOSE 4000
CMD ["node", "index.js"]