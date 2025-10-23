import React from 'react';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import Slide from '@mui/material/Slide';
import { TransitionProps } from '@mui/material/transitions';

const Transition = React.forwardRef(function Transition(
  props: TransitionProps & { children: React.ReactElement<any, any>; },
  ref: React.Ref<unknown>,
) {
  return <Slide direction="up" ref={ref} {...props} />;
});

interface ConfirmModalProps {
  open: boolean;
  title: string;
  content: string;
  onConfirm: () => void;
  onCancel: () => void;
}

const ConfirmModal: React.FC<ConfirmModalProps> = ({ open, title, content, onConfirm, onCancel }) => (
  <Dialog
    open={open}
    TransitionComponent={Transition}
    keepMounted
    onClose={onCancel}
    aria-describedby="confirm-dialog-description"
  >
    <DialogTitle>{title}</DialogTitle>
    <DialogContent>
      {content}
    </DialogContent>
    <DialogActions>
      <Button onClick={onCancel} color="inherit">Cancelar</Button>
      <Button onClick={onConfirm} color="primary" variant="contained">Confirmar</Button>
    </DialogActions>
  </Dialog>
);

export default ConfirmModal;
