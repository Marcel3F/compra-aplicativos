FROM mongo

COPY Data/seed/aplicativos.json /aplicativos.json
CMD mongoimport -u "compra-aplicativos" -p "e296cd9f" --authenticationDatabase "admin" --host mongodb --db compra-aplicativos --collection aplicativos --type json --file /aplicativos.json --jsonArray