import { Box, Typography, Container, Button, TextField, MenuItem, Snackbar, Alert } from '@mui/material';
import { useState } from 'react';
import { useOrders } from '../features/orders/contexts/OrderContext';

const bebidas = [
  { value: 'mojito', label: 'Mojito' },
  { value: 'caipirinha', label: 'Caipirinha' },
  { value: 'negroni', label: 'Negroni' },
  { value: 'fernet', label: 'Fernet con Coca' },
  { value: 'cerveza', label: 'Cerveza' },
];


export default function NewOrder() {
  const [drink, setDrink] = useState('');
  const [quantity, setQuantity] = useState(1);
  const [note, setNote] = useState('');
  const [success, setSuccess] = useState(false);
  const [openSnackbar, setOpenSnackbar] = useState(false);
  const { addPedido } = useOrders();

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    addPedido({ bebida: drink, cantidad: quantity, nota: note });
    setSuccess(true);
    setOpenSnackbar(true);
    setDrink('');
    setQuantity(1);
    setNote('');
  };

  return (
    <Container maxWidth="sm" sx={{ py: 8 }}>
      <Box
        sx={{
          bgcolor: 'background.paper',
          borderRadius: 3,
          boxShadow: 3,
          p: { xs: 3, md: 6 },
          textAlign: 'center',
        }}
      >
        <Typography variant="h4" color="primary" gutterBottom>
          Nuevo pedido
        </Typography>
        <Typography variant="body1" color="text.secondary" sx={{ mb: 4 }}>
          Selecciona tu bebida y completa los detalles.
        </Typography>
        <Box component="form" onSubmit={handleSubmit} sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
          <TextField
            select
            label="Bebida"
            value={drink}
            onChange={e => setDrink(e.target.value)}
            required
            fullWidth
            inputProps={{ 'aria-label': 'Bebida', 'aria-required': true }}
          >
            {bebidas.map(option => (
              <MenuItem key={option.value} value={option.value}>
                {option.label}
              </MenuItem>
            ))}
          </TextField>
          <TextField
            label="Cantidad"
            type="number"
            value={quantity}
            onChange={e => setQuantity(Number(e.target.value))}
            inputProps={{ min: 1, max: 20, 'aria-label': 'Cantidad', 'aria-required': true }}
            required
            fullWidth
          />
          <TextField
            label="Nota (opcional)"
            value={note}
            onChange={e => setNote(e.target.value)}
            multiline
            rows={2}
            fullWidth
            inputProps={{ 'aria-label': 'Nota' }}
          />
          <Button type="submit" variant="contained" color="primary" size="large" sx={{ mt: 2 }}>
            Enviar pedido
          </Button>
        </Box>

        <Snackbar
          open={openSnackbar}
          autoHideDuration={3000}
          onClose={() => setOpenSnackbar(false)}
          anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
        >
          <Alert onClose={() => setOpenSnackbar(false)} severity="success" sx={{ width: '100%' }}>
            ¡Pedido enviado!
          </Alert>
        </Snackbar>
      </Box>
    </Container>
  );

