import { INavData } from '@coreui/angular';

export const navItemsUsuario: INavData[] = [
  {
    title: true,
    name: 'PROVEEDORES'
  },
  {
    name: 'Proveedores',
    url: '/pages/proveedor-listar',
    iconComponent: { name: 'cil-pencil' }
  },
  {
    name: 'Cerrar Sesion',
    url: '/pages/logout',
    iconComponent: { name: 'cil-running' }
  }
];
