import React, { createContext, useContext, useState, ReactNode } from 'react';

export interface Pedido {
  id: number;
  bebida: string;
  cantidad: number;
  nota?: string;
  estado: string;
}

interface OrderContextType {
  pedidos: Pedido[];
  addPedido: (pedido: Omit<Pedido, 'id' | 'estado'>) => void;
}

const OrderContext = createContext<OrderContextType | undefined>(undefined);

export const OrderProvider = ({ children }: { children: ReactNode }) => {
  const [pedidos, setPedidos] = useState<Pedido[]>(() => {
    const saved = localStorage.getItem('pedidos');
    return saved ? JSON.parse(saved) : [];
  });

  const addPedido = (pedido: Omit<Pedido, 'id' | 'estado'>) => {
    const newPedido: Pedido = {
      ...pedido,
      id: Date.now(),
      estado: 'En preparación',
    };
    const updated = [newPedido, ...pedidos];
    setPedidos(updated);
    localStorage.setItem('pedidos', JSON.stringify(updated));
  };

  return (
    <OrderContext.Provider value={{ pedidos, addPedido }}>
      {children}
    </OrderContext.Provider>
  );
};

export const useOrders = () => {
  const context = useContext(OrderContext);
  if (!context) throw new Error('useOrders must be used within an OrderProvider');
  return context;
};
