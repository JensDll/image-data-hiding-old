# Image Data Hiding
Anwendung als Teil der Studienarbeit zur Kryptografie und Steganografie.

## Wie kann man es ausprobieren?
Projekt klonen und wenn Docker installiert ist, im Stammverzeichnis den folgenden Befehl ausführen:
```bash
docker-compose up --build
```
Gab es keinen Fehler, ist die Benutzeroberfläche jetzt zu erreichen unter `http://localhost:8000` und die Schnittstellen-Dokumentation unter `http://localhost:8001/swagger`.
Da es sich hierbei um eine Demoumgebung handelt, wird kein HTTPS verwendet. Dieses müsste für das Produktivsystem eingerichtet werden, damit geheime Nachrichten nicht als Klartext übertragen werden.
