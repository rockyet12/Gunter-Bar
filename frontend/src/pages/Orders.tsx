import { Box, Typography, Container, Button, List, ListItem, ListItemText, Chip } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useOrders } from '../features/orders/contexts/OrderContext';


export default function Orders() {
  const navigate = useNavigate();
  const { pedidos } = useOrders();
  return (
    <Container maxWidth="md" sx={{ py: 8 }}>
      <Box
        sx={{
          bgcolor: 'background.paper',
          borderRadius: 3,
          boxShadow: 3,
          p: { xs: 3, md: 6 },
          textAlign: 'center',
        }}
      >
        <Typography variant="h3" color="primary" gutterBottom>
          Tus pedidos
        </Typography>
        <Typography variant="h6" color="text.secondary" sx={{ mb: 4 }}>
          Aquí verás el historial y estado de tus pedidos.
        </Typography>
        <Button
          variant="contained"
          color="secondary"
          size="large"
          sx={{ mb: 4 }}
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
              <ListItem key={pedido.id} divider>
                <ListItemText
                  primary={`${pedido.cantidad} x ${pedido.bebida}`}
                  secondary={pedido.nota ? `Nota: ${pedido.nota}` : undefined}
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
                  variant="outlined"
                />
              </ListItem>
            ))
          )}
        </List>
      </Box>
    </Container>
  );
}
