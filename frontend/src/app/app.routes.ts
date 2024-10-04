import { Routes } from '@angular/router';
import { DefaultLayoutComponent } from './layout';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: '',
    component: DefaultLayoutComponent,
    data: {
      title: 'Home'
    },
    children: [      
      {
        path: 'pages',
        loadChildren: () => import('./views/pages/routes').then((m) => m.routes)
      },
      {
        path: 'orden-compra-mostrar/:id',
        loadComponent: () => import('./views/pages/orden-compra-mostrar/orden-compra-mostrar.component').then(m => m.OrdenCompraMostrarComponent),
        data: {
          title: 'Orden Compra Mostrar'
        }
      },      
      {
        path: 'orden-compra',
        loadComponent: () => import('./views/pages/orden-compra/orden-compra.component').then(m => m.OrdenCompraComponent),
        data: {
          title: 'Orden Compra'
        }
      },
      {
        path: 'conformidad-servicio',
        loadComponent: () => import('./views/pages/conformidad-servicio/conformidad-servicio.component').then(m => m.ConformidadServicioComponent),
        data: {
          title: 'Conformidad Servicio'
        }
      },
      {
        path: 'conformidad-servicio-crear/:id',
        loadComponent: () => import('./views/pages/conformidad-servicio-crear/conformidad-servicio-crear.component').then(m => m.ConformidadServicioCrearComponent),
        data: {
          title: 'Conformidad Servicio'
        }
      },
      {
        path: 'conformidad-servicio-mostrar/:id/:tipo',
        loadComponent: () => import('./views/pages/conformidad-servicio-mostrar/conformidad-servicio-mostrar.component').then(m => m.ConformidadServicioMostrarComponent),
        data: {
          title: 'Orden Compra Mostrar'
        }
      },
      {
        path: 'factura',
        loadComponent: () => import('./views/pages/factura/factura.component').then(m => m.FacturaComponent),
        data: {
          title: 'Conformidad Servicio'
        }
      },
      {
        path: 'anticipo-crear/:id',
        loadComponent: () => import('./views/pages/anticipo-crear/anticipo-crear.component').then(m => m.AnticipoCrearComponent),
        data: {
          title: 'Conformidad Servicio'
        }
      },
      {
        path: 'factura-crear/:id',
        loadComponent: () => import('./views/pages/factura-crear/factura-crear.component').then(m => m.FacturaCrearComponent),
        data: {
          title: 'Conformidad Servicio'
        }
      },
      {
        path: 'factura-mostrar/:id/:tipo',
        loadComponent: () => import('./views/pages/factura-mostrar/factura-mostrar.component').then(m => m.FacturaMostrarComponent),
        data: {
          title: 'Conformidad Servicio'
        }
      },
      {
        path: 'proveedor-mostrar/:id',
        loadComponent: () => import('./views/pages/proveedor-mostrar/proveedor-mostrar.component').then(m => m.ProveedorMostrarComponent),
        data: {
          title: 'Register Page'
        }
      },
      {
        path: 'proveedor-listar',
        loadComponent: () => import('./views/pages/proveedor-listar/proveedor-listar.component').then(m => m.ProveedorListarComponent),
        data: {
          title: 'Proveedor Listar'
        }
      },
    ]
  },  
  {
    path: 'login',
    loadComponent: () => import('./views/pages/login/login.component').then(m => m.LoginComponent),
    data: {
      title: 'Login Page'
    }
  },
  {
    path: 'logout',
    loadComponent: () => import('./views/pages/logout/logout.component').then(m => m.LogoutComponent),
    data: {
      title: 'Logout Page'
    }
  },
  {
    path: 'register',
    loadComponent: () => import('./views/pages/register/register.component').then(m => m.RegisterComponent),
    data: {
      title: 'Register Page'
    }
  },  
  {
    path: 'aprobacion/:id/:user',
    loadComponent: () => import('./views/pages/aprobacion/aprobacion.component').then(m => m.AprobacionComponent),
    data: {
      title: 'Aprobacion'
    }
  },
  {
    path: 'password-lost/:id',
    loadComponent: () => import('./views/pages/password-lost/password-lost.component').then(m => m.PasswordLostComponent),
    data: {
      title: 'Password'
    }
  },
  { path: '**', redirectTo: 'dashboard' }

];
