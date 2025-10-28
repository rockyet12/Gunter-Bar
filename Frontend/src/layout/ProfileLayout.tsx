import React from 'react';
import { Link, useNavigate, Outlet } from 'react-router-dom';
import { useAuth } from '../components/forms/AuthContext';
import { Button } from '../components/buttons';

interface ProfileLayoutProps {}

const ProfileLayout: React.FC<ProfileLayoutProps> = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  return (
    <div className="bg-background-dark font-display text-text-light">
      <div className="relative flex h-auto min-h-screen w-full flex-col group/design-root overflow-x-hidden">
        <div className="layout-container flex h-full grow flex-col">
          <div className="flex flex-1 justify-center py-5 lg:py-10 px-4 sm:px-6 lg:px-8">
            <div className="layout-content-container flex w-full max-w-7xl flex-1">
              {/* Sidebar - Hidden on mobile, visible on large screens */}
              <aside className="hidden lg:flex w-64 flex-col justify-between p-4 mr-8">
                <div className="flex flex-col gap-4">
                  {/* User Info */}
                  <div className="flex gap-3 items-center">
                    <div
                      className="bg-center bg-no-repeat aspect-square bg-cover rounded-full size-10"
                      style={{
                        backgroundImage: `url("${user?.profileImageUrl || 'https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=40&h=40&fit=crop&crop=face'}")`
                      }}
                    />
                    <div className="flex flex-col">
                      <h1 className="text-text-light text-base font-medium leading-normal font-display">
                        {user?.name} {user?.lastName}
                      </h1>
                      <p className="text-text-muted text-sm font-normal leading-normal font-display">
                        {user?.email}
                      </p>
                    </div>
                  </div>

                  {/* Navigation */}
                  <nav className="flex flex-col gap-2 mt-4">
                    <Link
                      to="/dashboard/profile"
                      className="flex items-center gap-3 px-3 py-2 rounded-lg bg-secondary hover:bg-secondary/90 transition-colors duration-200"
                    >
                      <span className="material-symbols-outlined text-primary text-[24px]">person</span>
                      <p className="text-primary text-sm font-medium leading-normal font-display">Información Personal</p>
                    </Link>
                    <Link
                      to="/dashboard/profile"
                      className="flex items-center gap-3 px-3 py-2 rounded-lg hover:bg-white/10 transition-colors duration-200"
                    >
                      <span className="material-symbols-outlined text-text-light text-[24px]">home</span>
                      <p className="text-text-light text-sm font-medium leading-normal font-display">Direcciones</p>
                    </Link>
                    <Link
                      to="/dashboard/profile"
                      className="flex items-center gap-3 px-3 py-2 rounded-lg hover:bg-white/10 transition-colors duration-200"
                    >
                      <span className="material-symbols-outlined text-text-light text-[24px]">receipt_long</span>
                      <p className="text-text-light text-sm font-medium leading-normal font-display">Historial de Pedidos</p>
                    </Link>
                  </nav>
                </div>

                <div className="flex flex-col gap-4">
                  {/* Logout Button */}
                  <Button
                    variant="outline"
                    size="sm"
                    onClick={handleLogout}
                    className="bg-secondary text-primary hover:bg-secondary/90 border-secondary"
                  >
                    Cerrar Sesión
                  </Button>

                  {/* Additional Links */}
                  <div className="flex flex-col gap-1">
                    <Link
                      to="/dashboard/profile"
                      className="flex items-center gap-3 px-3 py-2 rounded-lg hover:bg-white/10 transition-colors duration-200"
                    >
                      <span className="material-symbols-outlined text-text-light text-[24px]">settings</span>
                      <p className="text-text-light text-sm font-medium leading-normal font-display">Configuración</p>
                    </Link>
                    <Link
                      to="/dashboard/profile"
                      className="flex items-center gap-3 px-3 py-2 rounded-lg hover:bg-white/10 transition-colors duration-200"
                    >
                      <span className="material-symbols-outlined text-text-light text-[24px]">help</span>
                      <p className="text-text-light text-sm font-medium leading-normal font-display">Ayuda</p>
                    </Link>
                  </div>
                </div>
              </aside>

              {/* Main Content */}
              <main className="flex-1 bg-surface-dark rounded-xl p-6 md:p-10">
                <Outlet />
              </main>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProfileLayout;
