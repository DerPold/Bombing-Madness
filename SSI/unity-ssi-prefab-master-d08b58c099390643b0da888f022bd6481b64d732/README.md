unity-ssi-prefab enthält ein simples Beispielprojekt für die Verwendung von SSI mit Unity.
hierfür muss SSI bereits auf dem PC installiert sein

## Prefab anpassen
Das Prefab kann beliebige SSI Streams auslesen und in Unity zur Verfügung stellen.

0. Zum Testen Mit einer Standartversion von SSI ohne installierte IOM Treiber einfach den Haken bei "UseFakePipeline" im Prefab setzen
1. SSI Stream bereitstellen (Beispiel: SocketReader biomonitor-proc und biomonitor-procFake)
2. SSIParser konfigurieren über die SocketReader.xml
3. Wenn Events an SSI gesendet werden sollen einfach die SSIEventChannel.xml anpassen und in der biomonitor-proc sehen wie diese konsumiert werden

## Prefab verwenden
Um das Prefab zu verwenden einfach Rechtsklick auf die Assets und das ganze Exportieren und in eurem Unity Projekt wieder importieren.
Das testSSI.cs Skript enthält eine beispielhafte Verwendung der Schnittstelle

## WICHTIG
- Das prefab wird keine Daten empfangen, wenn die Netzwerkadapter aktiviert sind.
- UnityLifecycle enthält eine public DontDestroyOnLoad die auf true gesetzt werden kann wenn das prefab in mehreren Szenen verwendet werden soll
- Das Player API Compability Level muss zu .NET 2.0 geändert werden (Edit -> ProjectSettings -> Player)