#!/bin/bash
echo "BAR GUNTER - INICIANDO TODO"
echo "Backend    → http://localhost:5221"
echo "Cliente    → http://localhost:5173"
echo "Vendedor   → http://localhost:5174"
echo "Presiona CTRL+C para detener"
echo "=================================="

# Elige uno:
# npm run dev                    # ← con concurrently
docker-compose up --build        # ← con Docker (recomendado)