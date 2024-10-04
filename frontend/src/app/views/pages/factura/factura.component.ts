import { Component, OnInit } from '@angular/core';
import { TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, AlertComponent, ContainerComponent, RowComponent, ColComponent, CardGroupComponent, 
  FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective,
  TableModule, UtilitiesModule } from '@coreui/angular';
import { NgStyle, CommonModule } from '@angular/common';
import { IconDirective } from '@coreui/icons-angular';
import { ActivatedRoute, Router } from '@angular/router';
import { DocumentoService } from 'src/app/services/documento.service';
import { PagProveedorPipe } from 'src/app/pipes/pag-proveedor.pipe';

@Component({
  selector: 'app-factura',
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
    ButtonDirective,
    IconDirective,
    TableModule, 
    UtilitiesModule,
    NgStyle,
    PagProveedorPipe],
  templateUrl: './factura.component.html',
  styleUrl: './factura.component.scss'
})
export class FacturaComponent implements OnInit {
  errorValidacion: Boolean = false;
  flagExito: Boolean = false;
  flagFiltro: Boolean = false;
  msgError: string = "";
  listaHeader: any[] = [];
  listaPaginas: number[] = [];
  _rucProveedor: string = "";
  listaPedidos: any = [];
  pagina: number = 1;
  fecDesde: string = "";
  fecHasta: string = "";
  
  constructor(private _route: ActivatedRoute, private router: Router, private documentoService: DocumentoService){
    const dataTemp: any = sessionStorage.getItem("prov");
    const dataProv: any = JSON.parse(dataTemp);
    // console.log("prov", dataProv)
    this._rucProveedor = dataProv.licTradNum;

    const token: any = sessionStorage.getItem("tb");

    var fecha = new Date();
    var date = new Date();
    var primerDia = new Date(date.getFullYear(), date.getMonth(), 1);
    var ultimoDia = new Date(date.getFullYear(), date.getMonth() + 1, 0);
    
    // console.log("<br>El primer día es: "+primerDia.getDate().toString().padStart(2, "0"));
    // console.log("<br>El ultimo día es: "+ultimoDia.getDate());
    // const fecha = new Date().getFullYear().toString() + "-" + new Date().getMonth().toString().padStart(2 , "0") + "-01"
    console.log("fecha->", fecha.toISOString());
    
    this.fecDesde = fecha.toISOString().slice(0, 8) + primerDia.getDate().toString().padStart(2, "0");
    this.fecHasta = fecha.toISOString().slice(0, 8) + ultimoDia.getDate().toString().padStart(2, "0");

    if(token){
      this.listaHeader = [
        {
          label: "Tipo",
          key: "tipoDocumento",
          subKey: "",
          customClass: "",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "N° Sunat",
          key: "numAtCard",
          subKey: "",
          customClass: "",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Sucursal",
          key: "sucursal",
          subKey: "",
          customClass: "",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Fecha Reg.",
          key: "taxDate",
          subKey: "",
          customClass: "fecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Fecha Doc.",
          key: "docDate",
          subKey: "",
          customClass: "fecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Fecha Venc.",
          key: "docDueDate",
          subKey: "",
          customClass: "fecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Moneda",
          key: "docCur",
          subKey: "",
          customClass: "",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Fec. Venc. Garantia",
          key: "dueDate",
          subKey: "",
          customClass: "",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Fondo Garantia",
          key: "porFondoGar",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Detraccion",
          key: "detraccion",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Neto",
          key: "neto",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Importe",
          key: "docTotal",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Imp. Pago",          
          key: "paidToDate",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Saldo",
          key: "saldo",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },        
        {
          label: "Estado",
          key: "docStatus",
          subKey: "",
          customClass: "",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "*",
          key: "nroPedido",
          subKey: "",
          customClass: "btnEdit",
          type: "buttonSearch",
          value: "",
          visible: true
        },
      ];
    }else{
      this.router.navigateByUrl(`/login`, { replaceUrl: true });
    }
  }

  async buscar(){
    this.listaPaginas = [];
    this.listaPedidos = [];
    this.flagFiltro = false;
    const txtDesde = document.getElementById("txtDesde") as HTMLInputElement;
    const txtHasta = document.getElementById("txtHasta") as HTMLInputElement;
    const cmbEstado =  document.getElementById("cmbEstado") as HTMLSelectElement;

    const fi: string = txtDesde.value.replace("-","").replace("-","");
    const ff: string = txtHasta.value.replace("-","").replace("-","");

    await this.documentoService.getFacturas(this._rucProveedor, fi, ff, cmbEstado.value).toPromise().then(pedidos => {
      // console.log("pedidos->", pedidos);
      this.listaPedidos = pedidos;
      if(this.listaPedidos.length == 0){
        this.flagFiltro = true;
        return;
      }

      const residuo: number = this.listaPedidos.length % 10;
      const cociente: string = (this.listaPedidos.length / 10).toString().split('.')[0];
      const x: number = residuo == 0 ? 0 : 1;
      for(let i = 1; i <= Number(cociente) + x; i++){
        this.listaPaginas.push(i);
      }

      this.pagina = 1;

      for(let i = 0; i < this.listaPedidos.length; i++){
        this.listaPedidos[i].saldo = this.listaPedidos[i].saldo.toLocaleString('es-PE', {minimumFractionDigits: 2});
        this.listaPedidos[i].paidToDate = this.listaPedidos[i].paidToDate.toLocaleString('es-PE', {minimumFractionDigits: 2})
        this.listaPedidos[i].docTotal = this.listaPedidos[i].docTotal.toLocaleString('es-PE', {minimumFractionDigits: 2})
        this.listaPedidos[i].detraccion = this.listaPedidos[i].detraccion.toLocaleString('es-PE', {minimumFractionDigits: 2})
        this.listaPedidos[i].neto = this.listaPedidos[i].neto.toLocaleString('es-PE', {minimumFractionDigits: 2})
        this.listaPedidos[i].porFondoGar = this.listaPedidos[i].porFondoGar.toLocaleString('es-PE', {minimumFractionDigits: 2})
      }
    });
  }

  ngOnInit(): void {
    
  }

  searchRow(row: any){
    console.log(row);
    
    const docEntry: number = row.docEntry
    let tipo: string = "";
    if(row.docStatus.toString() == "PENDIENTE DE APROBACION" || row.docStatus.toString() == "RECHAZADO"){
      tipo = "B"
    }else if(row.tipoDocumento == "FACTURA"){
      tipo = "F"
    }else if(row.tipoDocumento == "ANTICIPO"){
      tipo = "A"
    }

    this.router.navigateByUrl(`/factura-mostrar/${ docEntry }/${ tipo }`, { replaceUrl: true });
  }

  selectPagina(num: number){
    this.pagina = num;
  }

  async descargar(){
    const txtDesde = document.getElementById("txtDesde") as HTMLInputElement;
    const txtHasta = document.getElementById("txtHasta") as HTMLInputElement;
    const cmbEstado =  document.getElementById("cmbEstado") as HTMLSelectElement;

    const fi: string = txtDesde.value.replace("-","").replace("-","");
    const ff: string = txtHasta.value.replace("-","").replace("-","");

    this.documentoService.getFacturasDownload(this._rucProveedor, fi,ff,cmbEstado.value).toPromise().then( data => {
      console.log("file", data);
      const archivo = data || "";
      var byteCharacters = atob(archivo.toString());
      var byteNumbers = new Array(byteCharacters.length);

      for (var i = 0; i < byteCharacters.length; i++) {
          byteNumbers[i] = byteCharacters.charCodeAt(i);
      }

      var byteArray = new Uint8Array(byteNumbers); 
 
      let filename = "pedido_" + fi + ff + ".xlsx";  
      let binaryData = [];
      binaryData.push(byteArray);
      
      let downloadLink = document.createElement('a');
      downloadLink.href = window.URL.createObjectURL(
      new Blob(binaryData, { type: 'blob' }));
      downloadLink.setAttribute('download', filename);
      document.body.appendChild(downloadLink);
      downloadLink.click();
    });
  }

}
