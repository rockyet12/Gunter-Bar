Propósito

Esta carpeta contiene copias de seguridad temporales de archivos que se deshabilitaron durante la depuración del proyecto Backend-Bar.

Archivo principal

- `Program.cs.disabled` — copia de seguridad del `Program.cs` duplicado que se movió aquí para evitar confusiones en tiempo de ejecución. No se compila mientras tenga la extensión `.disabled`.

Por qué existe

Durante la resolución de un error de arranque, se detectó una segunda copia de `Program.cs` que provocaba que se ejecutara una versión distinta a la que se estaba editando. Para evitar borrar trabajo y reducir el riesgo de ejecutar la copia equivocada, la copia se movió aquí.

Cómo restaurar (si necesitas volver a la versión de backup)

1. Haz una copia de seguridad del `Program.cs` actual:
   mv Backend-Bar/BarGunter.API/Program.cs Backend-Bar/BarGunter.API/Program.cs.bak
2. Mueve el backup al lugar original:
   mv Backend-Bar/backups/Program.cs.disabled Backend-Bar/BarGunter.API/Program.cs
3. Reconstruye y prueba:
   cd Backend-Bar/BarGunter.API && dotnet build && dotnet run

Eliminar la copia de seguridad

Si ya no necesitas la copia de seguridad, puedes eliminarla con:

rm Backend-Bar/backups/Program.cs.disabled

Contacto

Si hay dudas, revisa el historial de commits o pregunta antes de borrar backups.
