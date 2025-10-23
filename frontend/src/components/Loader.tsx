import React from 'react';
import CircularProgress from '@mui/material/CircularProgress';
import Box from '@mui/material/Box';

const Loader: React.FC<{ fullscreen?: boolean }> = ({ fullscreen = false }) => (
  <Box
    display="flex"
    justifyContent="center"
    alignItems="center"
    minHeight={fullscreen ? '100vh' : '200px'}
    width={fullscreen ? '100vw' : '100%'}
    position={fullscreen ? 'fixed' : 'relative'}
    top={fullscreen ? 0 : undefined}
    left={fullscreen ? 0 : undefined}
    bgcolor={fullscreen ? 'rgba(255,255,255,0.7)' : 'transparent'}
    zIndex={fullscreen ? 1300 : 'auto'}
  >
    <CircularProgress size={60} thickness={5} />
  </Box>
);

export default Loader;
