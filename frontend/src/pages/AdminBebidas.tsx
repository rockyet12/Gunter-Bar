import { Box, Typography, Container, Grid, Card, CardMedia, CardContent, CardActions, Button, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, CircularProgress, Alert } from '@mui/material';
import { useEffect, useState } from 'react';
import apiService from '../services/apiService';
import { User, UserRole } from '../types';

// Simulación de bebidas, en el futuro vendrá de la API
const bebidas = [
  {
    value: 'mojito',
    label: 'Mojito',
    img: 'https://images.unsplash.com/photo-1504674900247-0877df9cc836?auto=format&fit=crop&w=400&q=80',
    desc: 'Refrescante cóctel cubano con menta, lima y ron.'
  },
  {
    value: 'caipirinha',
    label: 'Caipirinha',
    img: 'https://images.unsplash.com/photo-1514361892635-cebb9b6b9d49?auto=format&fit=crop&w=400&q=80',
    desc: 'El clásico brasileño con cachaça, lima y azúcar.'
  },
  {
    value: 'negroni',
    label: 'Negroni',
    img: 'https://images.unsplash.com/photo-1464306076886-debca5e8a6b0?auto=format&fit=crop&w=400&q=80',
    desc: 'Bebida italiana con gin, vermut y Campari.'
  },
  {
    value: 'fernet',
    label: 'Fernet con Coca',
    img: 'https://images.unsplash.com/photo-1502741338009-cac2772e18bc?auto=format&fit=crop&w=400&q=80',
    desc: 'El clásico argentino: Fernet Branca y Coca-Cola.'
  },
  {
    value: 'cerveza',
    label: 'Cerveza',
    img: 'https://images.unsplash.com/photo-1513104890138-7c749659a591?auto=format&fit=crop&w=400&q=80',
    desc: 'Rubia, roja o negra, siempre fría.'
  },
];

export default function AdminBebidas() {
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);
  const [updatingId, setUpdatingId] = useState<number | null>(null);

  useEffect(() => {
    setLoading(true);
    apiService.getAllUsers()
      .then(res => setUsers(res.data || []))
      .catch(() => setError('Error al cargar usuarios'))
      .finally(() => setLoading(false));
  }, []);

  const handleAssignVendor = async (userId: number) => {
    setUpdatingId(userId);
    setError(null);
    setSuccess(null);
    try {
  await apiService.updateUserRole(userId, UserRole.Employee);
  setUsers(users => users.map(u => u.id === userId ? { ...u, role: UserRole.Employee } : u));
  setSuccess('Rol asignado correctamente');
    } catch {
      setError('No se pudo asignar el rol');
    } finally {
      setUpdatingId(null);
    }
  };

  return (
    <Container maxWidth="lg" sx={{ py: 8 }}>
      <Typography variant="h4" color="primary" gutterBottom align="center">
        Administración de Usuarios y Bebidas
      </Typography>
      <Box sx={{ textAlign: 'center', mb: 4 }}>
        <Button variant="contained" color="success" sx={{ fontWeight: 600 }}>
          Agregar nueva bebida
        </Button>
      </Box>
      <Typography variant="h5" sx={{ mb: 2 }}>Usuarios</Typography>
      {error && <Alert severity="error">{error}</Alert>}
      {success && <Alert severity="success">{success}</Alert>}
      {loading ? <CircularProgress /> : (
        <TableContainer component={Paper} sx={{ mb: 6 }}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>ID</TableCell>
                <TableCell>Nombre</TableCell>
                <TableCell>Email</TableCell>
                <TableCell>Rol</TableCell>
                <TableCell>Acción</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {users.map(user => (
                <TableRow key={user.id}>
                  <TableCell>{user.id}</TableCell>
                  <TableCell>{user.fullName}</TableCell>
                  <TableCell>{user.email}</TableCell>
                  <TableCell>{UserRole[user.role]}</TableCell>
                  <TableCell>
                    {user.role === UserRole.Customer ? (
                      <Button
                        variant="contained"
                        color="secondary"
                        size="small"
                        disabled={updatingId === user.id}
                        onClick={() => handleAssignVendor(user.id)}
                      >
                        {updatingId === user.id ? <CircularProgress size={18} /> : 'Asignar como Vendedor'}
                      </Button>
                    ) : (
                      <span>-</span>
                    )}
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      )}
      {/* ...bebidas UI... */}
      <Typography variant="h5" sx={{ mb: 2 }}>Bebidas</Typography>
      <Grid container spacing={4} justifyContent="center">
        {bebidas.map(bebida => (
          <Grid item xs={12} sm={6} md={4} lg={3} key={bebida.value}>
            <Card>
              <CardMedia
                component="img"
                height="180"
                image={bebida.img}
                alt={bebida.label}
              />
              <CardContent>
                <Typography variant="h6" gutterBottom>{bebida.label}</Typography>
                <Typography variant="body2" color="text.secondary">{bebida.desc}</Typography>
              </CardContent>
              <CardActions>
                <Button size="small" color="primary">Editar</Button>
                <Button size="small" color="error">Eliminar</Button>
                <Button size="small" color="secondary">Subir foto</Button>
              </CardActions>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Container>
  );
}
