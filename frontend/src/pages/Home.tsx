import { Box, Typography, Button, Container } from '@mui/material';
import { useNavigate } from 'react-router-dom';

export default function Home() {
  const navigate = useNavigate();
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
        <Typography variant="h2" color="primary" gutterBottom>
          ¡Bienvenido a Gunter Bar!
        </Typography>
        <Typography variant="h5" color="text.secondary" sx={{ mb: 4 }}>
          Haz tu pedido de bebidas de manera rápida y sencilla.
        </Typography>
        <Button
          variant="contained"
          color="secondary"
          size="large"
          sx={{ px: 6, py: 2, fontWeight: 600 }}
          onClick={() => navigate('/orders')}
        >
          Hacer un pedido
        </Button>
      </Box>
    </Container>
  );
}
