import React from 'react';
import { FieldError } from 'react-hook-form';

interface SelectProps {
  label?: string;
  placeholder?: string;
  error?: FieldError;
  icon?: React.ReactNode;
  className?: string;
  children: React.ReactNode;
  [key: string]: any;
}

const Select: React.FC<SelectProps> = ({
  label,
  placeholder,
  error,
  icon,
  className = '',
  children,
  ...props
}) => {
  return (
    <div className="mb-3">
      {label && (
        <label className="auth-form-label flex items-center gap-2">
          {icon}
          {label}
        </label>
      )}
      <select
        className={`auth-form-control ${error ? 'border-red-500' : ''} ${className}`}
        {...props}
      >
        {placeholder && (
          <option value="" disabled>
            {placeholder}
          </option>
        )}
        {children}
      </select>
      {error && <p className="text-red-400 text-sm mt-1">{error.message}</p>}
    </div>
  );
};

export default Select;
