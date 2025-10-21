import { createTheme } from '@mui/material/styles';
import { ThemeOptions } from '@mui/material/styles';

// Definición de colores base
const colors = {
  wine: {
    main: '#722F37',     // Vino principal
    light: '#8D3B44',    // Vino claro
    dark: '#5B2329',     // Vino oscuro
    contrastText: '#fff'
  },
  green: {
    main: '#2A9D8F',     // Verde principal
    light: '#34C5B5',    // Verde claro
    dark: '#207D73',     // Verde oscuro
    contrastText: '#fff'
  },
  orange: {
    main: '#E76F51',     // Naranja principal
    light: '#F4A261',    // Naranja claro
    dark: '#C54D31',     // Naranja oscuro
    contrastText: '#fff'
  }
};

const themeOptions: ThemeOptions = {
  palette: {
    primary: colors.wine,
    secondary: colors.green,
    warning: colors.orange,
    error: {
      main: '#d32f2f',
      light: '#ef5350',
      dark: '#c62828',
      contrastText: '#fff'
    },
    background: {
      default: '#f8f5f5',
      paper: '#fff'
    },
    text: {
      primary: '#2D3748',
      secondary: '#4A5568'
    }
  },
  typography: {
    fontFamily: [
      'Poppins',
      'Roboto',
      'Arial',
      'sans-serif'
    ].join(','),
    h1: {
      fontSize: '2.5rem',
      fontWeight: 600,
      color: colors.wine.main
    },
    h2: {
      fontSize: '2rem',
      fontWeight: 500,
      color: colors.wine.main
    },
    h3: {
      fontSize: '1.75rem',
      fontWeight: 500
    },
    h4: {
      fontSize: '1.5rem',
      fontWeight: 500
    },
    h5: {
      fontSize: '1.25rem',
      fontWeight: 500
    },
    h6: {
      fontSize: '1rem',
      fontWeight: 500
    },
    button: {
      textTransform: 'none',
      fontWeight: 500
    }
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          borderRadius: '8px',
          padding: '8px 24px'
        },
        contained: {
          boxShadow: 'none',
          '&:hover': {
            boxShadow: '0px 2px 4px rgba(0,0,0,0.2)'
          }
        }
      }
    },
    MuiCard: {
      styleOverrides: {
        root: {
          borderRadius: '12px',
          boxShadow: '0px 4px 8px rgba(0,0,0,0.1)'
        }
      }
    },
    MuiAppBar: {
      styleOverrides: {
        root: {
          backgroundColor: colors.wine.main
        }
      }
    },
    MuiTextField: {
      styleOverrides: {
        root: {
          '& .MuiOutlinedInput-root': {
            borderRadius: '8px'
          }
        }
      }
    }
  },
  shape: {
    borderRadius: 8
  }
};

const theme = createTheme(themeOptions);

export default theme;
