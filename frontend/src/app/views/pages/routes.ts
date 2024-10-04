import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '404',
    loadComponent: () => import('./page404/page404.component').then(m => m.Page404Component),
    data: {
      title: 'Page 404'
    }
  },
  {
    path: '500',
    loadComponent: () => import('./page500/page500.component').then(m => m.Page500Component),
    data: {
      title: 'Page 500'
    }
  },
  {
    path: 'login',
    loadComponent: () => import('./login/login.component').then(m => m.LoginComponent),
    data: {
      title: 'Login Page'
    }
  },
  {
    path: 'logout',
    loadComponent: () => import('./logout/logout.component').then(m => m.LogoutComponent),
    data: {
      title: 'Logout Page'
    }
  },
  {
    path: 'register',
    loadComponent: () => import('./register/register.component').then(m => m.RegisterComponent),
    data: {
      title: 'Register Page'
    }
  },
  {
    path: 'proveedor-mostrar/:id',
    loadComponent: () => import('./proveedor-mostrar/proveedor-mostrar.component').then(m => m.ProveedorMostrarComponent),
    data: {
      title: 'Register Page'
    }
  },
  {
    path: 'proveedor-listar',
    loadComponent: () => import('./proveedor-listar/proveedor-listar.component').then(m => m.ProveedorListarComponent),
    data: {
      title: 'Register Page'
    }
  },
  {
    path: 'orden-compra',
    loadComponent: () => import('./orden-compra/orden-compra.component').then(m => m.OrdenCompraComponent),
    data: {
      title: 'Orden de Compra'
    }
  },
  {
    path: 'factura',
    loadComponent: () => import('./factura/factura.component').then(m => m.FacturaComponent),
    data: {
      title: 'Factura'
    }
  },
  {
    path: 'conformidad-servicio',
    loadComponent: () => import('./conformidad-servicio/conformidad-servicio.component').then(m => m.ConformidadServicioComponent),
    data: {
      title: 'Conformidad Servicio'
    }
  },
  {
    path: 'conformidad-servicio-crear/:id',
    loadComponent: () => import('./conformidad-servicio-crear/conformidad-servicio-crear.component').then(m => m.ConformidadServicioCrearComponent),
    data: {
      title: 'Conformidad Servicio'
    }
  },
  // {
  //   path: 'password',
  //   loadComponent: () => import('./password/password.component').then(m => m.PasswordComponent),
  //   data: {
  //     title: 'Password'
  //   }
  // },
  {
    path: 'orden-compra-mostrar/:id',
    loadComponent: () => import('./orden-compra-mostrar/orden-compra-mostrar.component').then(m => m.OrdenCompraMostrarComponent),
    data: {
      title: 'Orden Compra Mostrar'
    }
  },
  {
    path: 'conformidad-servicio-mostrar/:id/:tipo',
    loadComponent: () => import('./conformidad-servicio-mostrar/conformidad-servicio-mostrar.component').then(m => m.ConformidadServicioMostrarComponent),
    data: {
      title: 'Conformidad Servicio Mostrar'
    }
  },
  {
    path: 'anticipo-crear/:id',
    loadComponent: () => import('./anticipo-crear/anticipo-crear.component').then(m => m.AnticipoCrearComponent),
    data: {
      title: 'Conformidad Servicio'
    }
  },
  {
    path: 'factura-crear/:id',
    loadComponent: () => import('./factura-crear/factura-crear.component').then(m => m.FacturaCrearComponent),
    data: {
      title: 'Conformidad Servicio'
    }
  },
  {
    path: 'factura-mostrar/:id/:tipo',
    loadComponent: () => import('./factura-mostrar/factura-mostrar.component').then(m => m.FacturaMostrarComponent),
    data: {
      title: 'Conformidad Servicio'
    }
  },
];
