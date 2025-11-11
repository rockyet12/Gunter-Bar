#!/bin/bash

# Script para hacer build de ambos frontends
echo "🔨 Construyendo Gunter Bar - Ambos Frontends"

echo "📱 Construyendo frontend de clientes..."
cd Frontend && npm run build
if [ $? -ne 0 ]; then
    echo "❌ Error en build del frontend de clientes"
    exit 1
fi

echo "🏪 Construyendo frontend de vendedores..."
cd ../seller-frontend && npm run build
if [ $? -ne 0 ]; then
    echo "❌ Error en build del frontend de vendedores"
    exit 1
fi

echo ""
echo "✅ ¡Ambos frontends construidos exitosamente!"
echo "📁 Builds disponibles en:"
echo "   👥 Frontend/dist/"
echo "   🏪 seller-frontend/dist/"
echo ""
echo "🚀 Listo para despliegue"