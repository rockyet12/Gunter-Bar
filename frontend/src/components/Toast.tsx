import React from 'react';
import Snackbar from '@mui/material/Snackbar';
import MuiAlert, { AlertColor } from '@mui/material/Alert';
import Slide from '@mui/material/Slide';

const SlideTransition = (props: any) => {
  return <Slide {...props} direction="up" />;
};

interface ToastProps {
  open: boolean;
  message: string;
  severity?: AlertColor;
  onClose: () => void;
  duration?: number;
}

const Toast: React.FC<ToastProps> = ({ open, message, severity = 'success', onClose, duration = 3000 }) => (
  <Snackbar
    open={open}
    autoHideDuration={duration}
    onClose={onClose}
    anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
    TransitionComponent={SlideTransition}
  >
    <MuiAlert elevation={6} variant="filled" onClose={onClose} severity={severity} sx={{ width: '100%' }}>
      {message}
    </MuiAlert>
  </Snackbar>
);

export default Toast;
