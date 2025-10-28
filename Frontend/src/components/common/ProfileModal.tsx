import React from 'react';
import './ProfileModal.css';

interface ProfileModalProps {
  isOpen: boolean;
  onClose: () => void;
}

const ProfileModal: React.FC<ProfileModalProps> = ({ isOpen, onClose }) => {
  console.log('ProfileModal render, isOpen:', isOpen);
  
  if (!isOpen) return null;

  return (
    <div className="profile-modal-overlay">
      <div className="profile-modal-content">
        <div className="profile-modal-header">
          <h2 className="profile-modal-title">Mi Perfil</h2>
          <button onClick={onClose} className="profile-modal-close">
            ×
          </button>
        </div>
        <div className="profile-modal-body">
          <p className="profile-modal-message">¡MODAL FUNCIONANDO! Si ves esto, el modal se abre correctamente.</p>
          <button
            onClick={onClose}
            className="profile-modal-button"
          >
            Cerrar
          </button>
        </div>
      </div>
    </div>
  );
};

export default ProfileModal;
