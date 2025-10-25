import { Box, Typography, Container, Button, List, ListItem, ListItemText, Chip } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useOrders } from '../features/orders/contexts/OrderContext';


export default function Orders() {
  const navigate = useNavigate();
  const { pedidos } = useOrders();
  return (
    <Box sx={{ minHeight: '100vh', background: 'linear-gradient(120deg, #f5f7fa 0%, #c3cfe2 100%)', py: 8, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
      <Container maxWidth="sm">
        <Box sx={{ p: 4, bgcolor: 'white', borderRadius: 4, boxShadow: 6, textAlign: 'center', animation: 'fadeDown 1s' }}>
          <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', mb: 2 }}>
            <img src="/logo192.png" alt="Logo" style={{ width: 40, marginRight: 8, opacity: 0.85 }} />
            <Typography variant="h4" color="primary" sx={{ fontWeight: 700, letterSpacing: 1 }}>
              Tus pedidos
            </Typography>
          </Box>
          <Typography variant="h6" color="text.secondary" sx={{ mb: 4 }}>
            Aquí verás el historial y estado de tus pedidos.
          </Typography>
          <Button
            variant="contained"
            color="secondary"
            size="large"
            sx={{ mb: 4, fontWeight: 600, fontSize: 18, letterSpacing: 1 }}
            onClick={() => navigate('/orders/new')}
          >
            Nuevo pedido
          </Button>
          <List sx={{ maxWidth: 500, mx: 'auto', textAlign: 'left' }}>
            {pedidos.length === 0 ? (
              <Typography color="text.secondary" sx={{ textAlign: 'center', mt: 2 }}>
                No tienes pedidos aún.
              </Typography>
            ) : (
              pedidos.map((pedido) => (
                <ListItem key={pedido.id} divider sx={{ transition: 'background 0.2s', '&:hover': { background: '#f5f7fa' } }}>
                  <ListItemText
                    primary={<span style={{ fontWeight: 600 }}>{`${pedido.cantidad} x ${pedido.bebida}`}</span>}
                    secondary={pedido.nota ? <span style={{ color: '#888' }}>{`Nota: ${pedido.nota}`}</span> : undefined}
                  />
                  <Chip
                    label={pedido.estado}
                    color={
                      pedido.estado === 'Listo'
                        ? 'success'
                        : pedido.estado === 'En preparación'
                        ? 'warning'
                        : 'default'
                    }
                    variant="filled"
                    sx={{ fontWeight: 600, fontSize: 15 }}
                  />
                </ListItem>
              ))
            )}
          </List>
        </Box>
      </Container>
    </Box>
  );
}
