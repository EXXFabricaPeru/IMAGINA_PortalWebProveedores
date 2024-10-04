import { Component } from '@angular/core';
import { TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, AlertComponent, ContainerComponent, RowComponent, ColComponent, CardGroupComponent, 
  FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective,
  TableModule, UtilitiesModule } from '@coreui/angular';
import { NgStyle, CommonModule } from '@angular/common';
import { IconDirective } from '@coreui/icons-angular';
import { ActivatedRoute, Router } from '@angular/router';
import { DocumentoService } from 'src/app/services/documento.service';
import { DomSanitizer } from '@angular/platform-browser';
import { MaestroService } from 'src/app/services/maestro.service';

@Component({
  selector: 'app-factura-mostrar',
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
    NgStyle],
  templateUrl: './factura-mostrar.component.html',
  styleUrl: './factura-mostrar.component.scss'
})
export class FacturaMostrarComponent {
  factura: any = {docNum: ""};
  listaPedidos:any = [];
  listaHeader: any = [];
  listaProvFact: any = [];
  aplicaFactoring: Boolean = false;
  validoFactoring: Boolean = false;
  flagDiasConfig: Boolean = false;
  dias: number = 0;

  constructor(private maestroService: MaestroService, private _route: ActivatedRoute, private router: Router, private documentoService: DocumentoService, private sanitizer: DomSanitizer){
    // const dataTemp: any = localStorage.getItem("prov");
    const dataTemp: any = sessionStorage.getItem("prov");
    const dataProv: any = JSON.parse(dataTemp);

    maestroService.getConfiguracion().toPromise().then(data => {
      console.log("configuracion", data);
       const dataConfig: any = data;
      for(let i=0; i<dataConfig.length; i++){
        if(i==4){
          this.dias = Number(dataConfig[i].descripcion);
          this.flagDiasConfig = dataConfig[i].flag01;
        }
      }
    })

    this.listaHeader = [
      {
        label: "Descripción",
        key: "dscription",
        subKey: "",
        customClass: "",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "Cantidad",
        key: "quantity",
        subKey: "",
        customClass: "derecha",
        type: "",
        value: "",
        visible: true
      },
      // {
      //   label: "Fec. Entrega",
      //   key: "shipDate",
      //   subKey: "",
      //   customClass: "fecha",
      //   type: "",
      //   value: "",
      //   visible: true
      // },
      {
        label: "Impuesto",
        key: "taxCode",
        subKey: "",
        customClass: "",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "Prec. Unit",
        key: "price",
        subKey: "",
        customClass: "derecha",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "Precio Total",
        key: "lineTotal",
        subKey: "",
        customClass: "derecha",
        type: "",
        value: "",
        visible: true
      },
    ];
    
    let docEntry: string | null;

    docEntry = this._route.snapshot.paramMap.get("id") || "";

    this.obtenerFactura(docEntry);
  }

  async obtenerFactura(id: string){
    let tipo: string | null;
    tipo = this._route.snapshot.paramMap.get("tipo") || "";

    if(tipo == "F"){
      await this.documentoService.getFacturaId(id).toPromise().then(data => {
        console.log("factura", data);
        this.factura = data;
        this.listaPedidos = this.factura.detallePedido;
        for(let i = 0; i < this.listaPedidos.length; i++){
          this.listaPedidos[i].sucursal = this.factura.sucursal;
        }
      });
    }

    if(tipo == "A"){
      await this.documentoService.getAnticipoId(id).toPromise().then(data => {
        console.log("factura", data);
        this.factura = data;
        this.listaPedidos = this.factura.detallePedido;
        for(let i = 0; i < this.listaPedidos.length; i++){
          this.listaPedidos[i].sucursal = this.factura.sucursal;
        }
      });
    }

    if(tipo == "B"){
      await this.documentoService.getBorradorId(id).toPromise().then(data => {
        console.log("factura", data);
        this.factura = data;
        this.listaPedidos = this.factura.detallePedido;
        for(let i = 0; i < this.listaPedidos.length; i++){
          this.listaPedidos[i].sucursal = this.factura.sucursal;
        }
      });
    }

    const fecha: Date = new Date(this.factura.taxDate);
    const fehoy: Date = new Date();
    fehoy.setHours(0)
    fehoy.setMinutes(0)
    fehoy.setSeconds(0)
    fehoy.setMilliseconds(0)

    console.log(fehoy);
    console.log(fecha);
    
    var diff = fehoy.getTime() - fecha.getTime();
    console.log("diferencia", diff / 1000 / 60 / 60 / 24 );
    
    if(this.factura.docStatus == "Pendiente de Pago" && (diff <= this.dias && this.flagDiasConfig)){
      this.validoFactoring = true;
    }
  }

  capturarFile(code: any){
    console.log("Code", code);
    console.log("factura", this.factura);
    
    let nombre: string = "";
    let archivo: string = "";
    switch (code)
    {
      case "1":
        nombre = this.factura.nomArchivo;
        archivo = this.factura.archivo.toString().replace("\\\\", "||").replace("\\", "|").replace("\\", "|");
        break;
      case "2":
        nombre = this.factura.nomArchivo2;
        archivo = this.factura.archivo2.toString().replace("\\\\", "||").replace("\\", "|").replace("\\", "|");
        break;
      case "3":
        nombre = this.factura.nomArchivo3;
        archivo = this.factura.archivo3.toString().replace("\\\\", "||").replace("\\", "|").replace("\\", "|");
        break;
      case "4":
        nombre = this.factura.nomArchivo4;
        archivo = this.factura.archivo4.toString().replace("\\\\", "||").replace("\\", "|").replace("\\", "|");
        break;
      case "5":
        nombre = this.factura.nomArchivo5;
        archivo = this.factura.archivo5.toString().replace("\\\\", "||").replace("\\", "|").replace("\\", "|");
        break;
    }

    console.log("archivo", archivo);
    this.documentoService.downloadFile(archivo).toPromise().then(data => {
      const archivo: string = data || "";
      var byteCharacters = atob(archivo);
      var byteNumbers = new Array(byteCharacters.length);

      for (var i = 0; i < byteCharacters.length; i++) {
          byteNumbers[i] = byteCharacters.charCodeAt(i);
      }

      var byteArray = new Uint8Array(byteNumbers); 
 
      let filename = nombre;  
      let binaryData = [];
      binaryData.push(byteArray);
      
      let downloadLink = document.createElement('a');
      downloadLink.href = window.URL.createObjectURL(
      new Blob(binaryData, { type: 'blob' }));
      downloadLink.setAttribute('download', filename);
      document.body.appendChild(downloadLink);
      downloadLink.click();
      // this.confirmDialog = true;
    })
  }

  async aplicaProvFactoring(){
    const cmbAplFact = document.getElementById("cmbAplFact") as HTMLSelectElement
    const cmbProFact = document.getElementById("cmbProFact") as HTMLSelectElement
    if(cmbAplFact.value == "Y"){
      this.aplicaFactoring = true;
    }else{
      this.aplicaFactoring = false;
      cmbProFact.selectedIndex = 0;
    }
  }
}
