import { Box, Typography, Container, Grid, Card, CardMedia, CardContent, CardActions, Button, CircularProgress, Chip, IconButton } from '@mui/material';
import { DataGrid } from '@mui/x-data-grid';
import { Edit, Delete, PhotoCamera } from '@mui/icons-material';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import styled from 'styled-components';
import ConfirmModal from '../components/ConfirmModal';
import Toast from '../components/Toast';
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
  const [dniSearch, setDniSearch] = useState('');
  const [loading, setLoading] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);
  const [pendingRole, setPendingRole] = useState<{ userId: number, role: UserRole } | null>(null);
  const [updatingId, setUpdatingId] = useState<number | null>(null);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [pendingDeleteId, setPendingDeleteId] = useState<number | null>(null);

  useEffect(() => {
    setLoading(true);
    apiService.getAllUsers()
      .then(res => setUsers(res.data || []))
      .catch(() => toast.error('Error al cargar usuarios'))
      .finally(() => setLoading(false));
  }, []);

  const handleAssignRole = (userId: number, role: UserRole) => {
    setPendingRole({ userId, role });
    setModalOpen(true);
  };

  const confirmAssignRole = async () => {
    if (!pendingRole) return;
    const { userId, role } = pendingRole;
    setUpdatingId(userId);
    setModalOpen(false);
    try {
      await apiService.updateUserRole(userId, role);
      setUsers(users => users.map(u => u.id === userId ? { ...u, role } : u));
      toast.success('Rol asignado correctamente');
    } catch {
      toast.error('No se pudo asignar el rol');
    } finally {
      setUpdatingId(null);
    }
  };

  const handleDeleteUser = (userId: number) => {
    setPendingDeleteId(userId);
    setDeleteModalOpen(true);
  };

  const confirmDeleteUser = async () => {
    if (!pendingDeleteId) return;
    setUpdatingId(pendingDeleteId);
    setDeleteModalOpen(false);
    try {
      await apiService.deleteUser(pendingDeleteId);
      setUsers(users => users.filter(u => u.id !== pendingDeleteId));
      toast.success('Usuario eliminado correctamente');
    } catch {
      toast.error('No se pudo eliminar el usuario');
    } finally {
      setUpdatingId(null);
      setPendingDeleteId(null);
    }
  };

  return (
    <StyledBg>
      <Container maxWidth="lg" sx={{ py: 8 }}>
        <Typography variant="h4" color="primary" gutterBottom align="center" sx={{ fontWeight: 700 }}>
          Administración de Usuarios y Bebidas
        </Typography>
        <Box sx={{ textAlign: 'center', mb: 4 }}>
          <Button variant="contained" color="success" sx={{ fontWeight: 600 }} startIcon={<PhotoCamera />}>
            Agregar nueva bebida
          </Button>
        </Box>
        <Typography variant="h5" sx={{ mb: 2 }}>Usuarios</Typography>
        <Box sx={{ maxWidth: 300, mb: 2 }}>
          <TextField
            label="Buscar por DNI"
            value={dniSearch}
            onChange={e => setDniSearch(e.target.value.replace(/[^0-9]/g, ''))}
            fullWidth
            placeholder="Ej: 12345678"
            inputProps={{ maxLength: 20 }}
          />
        </Box>
        <ConfirmModal
          open={modalOpen}
          title="Confirmar cambio de rol"
          content={pendingRole ? `¿Seguro que quieres asignar el rol de ${pendingRole.role === UserRole.Employee ? 'Vendedor' : 'Jefe de Ventas'} a este usuario?` : ''}
          onConfirm={confirmAssignRole}
          onCancel={() => setModalOpen(false)}
        />
        <ConfirmModal
          open={deleteModalOpen}
          title="Confirmar eliminación"
          content={pendingDeleteId ? '¿Seguro que quieres eliminar este usuario? Esta acción no se puede deshacer.' : ''}
          onConfirm={confirmDeleteUser}
          onCancel={() => setDeleteModalOpen(false)}
        />
        <ToastContainer position="top-center" autoClose={2500} hideProgressBar theme="colored" aria-label="notificación" />
        {loading ? <CircularProgress /> : (
          <Box sx={{ height: 400, mb: 6 }}>
            <DataGrid
              rows={users
                .filter(u => dniSearch.length === 0 || (u.dni && u.dni.includes(dniSearch)))
                .map(u => ({ id: u.id, nombre: u.fullName, email: u.email, dni: u.dni || '', rol: u.role, rawRole: u.role }))}
              columns={[
                { field: 'id', headerName: 'ID', width: 80 },
                { field: 'nombre', headerName: 'Nombre', width: 180 },
                { field: 'email', headerName: 'Email', width: 220 },
                { field: 'dni', headerName: 'DNI', width: 120 },
                {
                  field: 'rol',
                  headerName: 'Rol',
                  width: 160,
                  renderCell: (params) => {
                    const role = params.row.rol;
                    let color = 'default';
                    let label = '';
                    if (role === UserRole.Admin) { color = 'error'; label = 'Admin'; }
                    else if (role === UserRole.Employee) { color = 'success'; label = 'Vendedor'; }
                    else if (role === UserRole.SalesManager) { color = 'primary'; label = 'Jefe de Ventas'; }
                    else { color = 'default'; label = 'Cliente'; }
                    return <Chip label={label} color={color} variant="outlined" sx={{ fontWeight: 600 }} />;
                  }
                },
                {
                  field: 'acciones',
                  headerName: 'Acción',
                  width: 340,
                  renderCell: (params) => {
                    const userId = params.row.id;
                    const userRole = params.row.rawRole;
                    return (
                      <Box sx={{ display: 'flex', gap: 1 }}>
                        {userRole !== UserRole.Admin && (
                          <Button
                            variant="contained"
                            color="secondary"
                            size="small"
                            disabled={updatingId === userId}
                            onClick={() => handleAssignRole(userId, UserRole.Employee)}
                            startIcon={<Edit />}
                          >
                            {updatingId === userId ? <CircularProgress size={18} /> : 'Asignar Vendedor'}
                          </Button>
                        )}
                        {userRole !== UserRole.Admin && (
                          <Button
                            variant="contained"
                            color="primary"
                            size="small"
                            disabled={updatingId === userId}
                            onClick={() => handleAssignRole(userId, UserRole.SalesManager)}
                            startIcon={<Edit />}
                          >
                            {updatingId === userId ? <CircularProgress size={18} /> : 'Asignar Jefe'}
                          </Button>
                        )}
                        {userRole !== UserRole.Admin && (
                          <Button
                            variant="contained"
                            color="error"
                            size="small"
                            disabled={updatingId === userId}
                            onClick={() => handleDeleteUser(userId)}
                            startIcon={<Delete />}
                          >
                            {updatingId === userId ? <CircularProgress size={18} /> : 'Eliminar'}
                          </Button>
                        )}
                      </Box>
                    );
                  }
                }
              ]}
              pageSizeOptions={[5]}
              initialState={{ pagination: { paginationModel: { pageSize: 5 } } }}
              disableRowSelectionOnClick
            />
          </Box>
        )}
        <Typography variant="h5" sx={{ mb: 2 }}>Bebidas</Typography>
        <Grid container spacing={4} justifyContent="center">
          {bebidas.map(bebida => (
            <Grid item xs={12} sm={6} md={4} lg={3} key={bebida.value}>
              <Card sx={{ boxShadow: 3, borderRadius: 3 }}>
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
                  <Button size="small" color="primary" startIcon={<Edit />}>Editar</Button>
                  <Button size="small" color="error" startIcon={<Delete />}>Eliminar</Button>
                  <Button size="small" color="secondary" startIcon={<PhotoCamera />}>Subir foto</Button>
                </CardActions>
              </Card>
            </Grid>
          ))}
        </Grid>
      </Container>
    </StyledBg>
  );
}

// Fondo sutil con styled-components
const StyledBg = styled.div`
  min-height: 100vh;
  background: linear-gradient(120deg, #f5f7fa 0%, #c3cfe2 100%);
`;
