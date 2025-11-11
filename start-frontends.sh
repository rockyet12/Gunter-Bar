#!/bin/bash

# Script para ejecutar ambos frontends simultáneamente
echo "🚀 Iniciando Gunter Bar - Frontend Dual"

# Función para manejar la señal de interrupción
cleanup() {
    echo ""
    echo "🛑 Deteniendo servidores..."
    kill 0
    exit 0
}

# Capturar Ctrl+C
trap cleanup SIGINT

# Iniciar el frontend de clientes
echo "📱 Iniciando frontend de clientes (puerto 5173)..."
cd Frontend && npm run dev > ../logs/customer-frontend.log 2>&1 &
CUSTOMER_PID=$!

# Esperar un momento para que el primer servidor inicie
sleep 3

# Iniciar el frontend de vendedores
echo "🏪 Iniciando frontend de vendedores (puerto 5174)..."
cd seller-frontend && npm run dev > ../logs/seller-frontend.log 2>&1 &
SELLER_PID=$!

echo ""
echo "✅ Ambos frontends están ejecutándose:"
echo "   👥 Clientes:    http://localhost:5173"
echo "   🏪 Vendedores: http://localhost:5174"
echo ""
echo "📝 Presiona Ctrl+C para detener ambos servidores"

# Esperar a que terminen los procesos
wait $CUSTOMER_PID $SELLER_PID