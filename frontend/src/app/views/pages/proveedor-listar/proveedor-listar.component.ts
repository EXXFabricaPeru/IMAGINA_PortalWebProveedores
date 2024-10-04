import { Component } from '@angular/core';
import { TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, AlertComponent, ContainerComponent, RowComponent, ColComponent, CardGroupComponent, 
  FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective,PaginationComponent, PageItemComponent,
  TableModule, UtilitiesModule } from '@coreui/angular';
import { NgStyle, CommonModule } from '@angular/common';
import { IconDirective } from '@coreui/icons-angular';
import { ActivatedRoute, Router } from '@angular/router';

import { ProveedorService } from 'src/app/services/proveedor.service';

@Component({
  selector: 'app-proveedor-listar',
  standalone: true,
  imports: [TextColorDirective,
    CardComponent,
    CardHeaderComponent,
    CardBodyComponent,
    CommonModule,
    AlertComponent,
    RowComponent,
    ColComponent,
    InputGroupComponent,
    CardGroupComponent,
    ContainerComponent,
    FormDirective,
    InputGroupTextDirective, 
    FormControlDirective,
    PaginationComponent, PageItemComponent,
    ButtonDirective,
    IconDirective,
    TableModule, 
    UtilitiesModule,
    NgStyle
  ],
  templateUrl: './proveedor-listar.component.html',
  styleUrl: './proveedor-listar.component.scss'
})
export class ProveedorListarComponent {
  flagFiltro = false;
  listaHeader: any;
  listaProveedor: any;
  pagina: number = 1;
  listaPaginas: number[] = [];

  constructor(private proveedorService: ProveedorService, private router: Router){
    this.listaHeader = [
      {
        label: "Código",
        key: "cardCode",
        subKey: "",
        customClass: "",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "Razón Social",
        key: "cardName",
        subKey: "",
        customClass: "",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "R.U.C",
        key: "licTradNum",
        subKey: "",
        customClass: "",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "e-mail",
        key: "emailAddress",
        subKey: "",
        customClass: "",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "Estado",
        key: "estado",
        subKey: "",
        customClass: "",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "FechaSol",
        key: "fechaSol",
        subKey: "",
        customClass: "fecha",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "FechaApr",
        key: "fechaApr",
        subKey: "",
        customClass: "fecha",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "*",
        key: "estado",
        subKey: "",
        customClass: "",
        type: "buttonSearch",
        value: "",
        visible: true
      }
    ];
  }

  buscar(){
    this.flagFiltro = false;
    this.listaProveedor = [];
    this.listaPaginas = [];
    const txtValor =  document.getElementById("txtValor") as HTMLInputElement;
    const cmbEstado =  document.getElementById("cmbEstado") as HTMLSelectElement;
    this.proveedorService.getListaProveedor(txtValor.value, cmbEstado.value).toPromise().then(data => {
      // console.log(data);
      
      this.listaProveedor = data;
      if(this.listaProveedor.length == 0){
        this.flagFiltro = true;
        return;
      }
      const residuo: number = this.listaProveedor.length % 10;
      const cociente: string = (this.listaProveedor.length / 10).toString().split('.')[0];
      const x: number = residuo == 0 ? 0 : 1;
      for(let i = 1; i <= Number(cociente) + x; i++){
        this.listaPaginas.push(i);
      }

      this.pagina = 1;
    })
  }

  obtenerProveedor(row: any){
    this.router.navigateByUrl(`/proveedor-mostrar/${row.cardCode}`, { replaceUrl: true });
  }

  selectPagina(num: number){
    
  }
}
