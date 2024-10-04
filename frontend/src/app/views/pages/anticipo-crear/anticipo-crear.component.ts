import { Component } from '@angular/core';
import { TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, AlertComponent, ContainerComponent, RowComponent, ColComponent, CardGroupComponent, 
  FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective,
  TableModule, UtilitiesModule } from '@coreui/angular';
import { NgStyle, CommonModule } from '@angular/common';
import { IconDirective } from '@coreui/icons-angular';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { DocumentoService } from 'src/app/services/documento.service';
import { MaestroService } from 'src/app/services/maestro.service';
import * as converter from 'xml-js';

@Component({
  selector: 'app-anticipo-crear',
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
  templateUrl: './anticipo-crear.component.html',
  styleUrl: './anticipo-crear.component.scss'
})
export class AnticipoCrearComponent {
  errorValidacion: Boolean = false;
  flagExito: Boolean = false;
  flagError: Boolean = false;
  modalDialog: Boolean = false;
  aplicaFactoring: Boolean = false;
  flagLoad: Boolean = false;

  msgError?: string = "";
  listaHeader: any[] = [];
  listaHeaderDocs: any[] = [];
  _rucProveedor: string = "";
  listaPedidos: any = [];
  listaPedidosAux: any = [];
  listaCS: any = [];
  listaProvFact: any = [];

  pagina: number = 1;
  pedido: any = {docStatus: ""};  
  userProv: string = "";

  inputXml : any;
  archivo1: any;
  nomArchivo1: string = "";
  archivo2: any;
  nomArchivo2: string = "";
  archivo3: any;
  nomArchivo3: string = "";
  archivo4: any;
  nomArchivo4: string = "";
  archivo5: any;
  nomArchivo5: string = "";

  constructor(private _route: ActivatedRoute, private router: Router, private documentoService: DocumentoService,  private maestroService: MaestroService, private sanitizer: DomSanitizer){
    // const dataTemp: any = localStorage.getItem("prov");
    const dataTemp: any = sessionStorage.getItem("prov");
    const dataProv: any = JSON.parse(dataTemp);
    // console.log("prov", dataProv)
    this._rucProveedor = dataProv.licTradNum;
    this.userProv = dataProv.licTradNum;

    this.listaHeader = [
      // {
      //   label: "Código",
      //   key: "itemCode",
      //   subKey: "",
      //   customClass: "",
      //   type: "",
      //   value: "",
      //   visible: true
      // },
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
        key: "pendQuantity",
        subKey: "",
        customClass: "derecha",
        type: "",
        value: "",
        visible: true
      },
      // {
      //   label: "U.M",
      //   key: "unitMsr",
      //   subKey: "",
      //   customClass: "",
      //   type: "",
      //   value: "",
      //   visible: true
      // },
      {
        label: "Fec. Entrega",
        key: "shipDate",
        subKey: "",
        customClass: "fecha",
        type: "",
        value: "",
        visible: true
      },
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

    this.obtenerPedido(docEntry);
  }

  async obtenerPedido(id: string){
    await this.documentoService.getPedidoId(id).toPromise().then(data => {
      console.log("pedido", data);
      this.pedido = data;
      this.listaPedidos = this.pedido.detallePedido;
      for(let i = 0; i < this.listaPedidos.length; i++){
        this.listaPedidos[i].sucursal = this.pedido.sucursal;
      }
    })
  }

  capturarFile(event: any, tipo: string){
    const fileCopy = event.target.files[0];
    // console.log(fileCopy);
    
    this.extraerBase64(fileCopy).then((file: any) => {
      // console.log(file);

      switch(tipo){
        case "1":
          this.archivo1 = (file.base).split(',')[1];
          this.nomArchivo1 = fileCopy.name;
          break;
        case "2":
          this.archivo2 = (file.base).split(',')[1];
          this.nomArchivo2 = fileCopy.name;
          break;
        case "3":
          this.archivo3 = (file.base).split(',')[1];
          this.nomArchivo3 = fileCopy.name;
          break;
        case "4":
          this.archivo4 = (file.base).split(',')[1];
          this.nomArchivo4 = fileCopy.name;
          break;
        case "5":
          this.archivo5 = (file.base).split(',')[1];
          this.nomArchivo5 = fileCopy.name;
          break;
      }
      
      // console.log(this.nomArchivo, this.archivo);
      this.msgError = "";
      this.errorValidacion = false;
    })
  }

  extraerBase64 = async ( $event: any ) => new Promise(( resolve, reject) => {
    let database: any;
    try {
      const unsafeFile = window.URL.createObjectURL($event);
      const file = this.sanitizer.bypassSecurityTrustUrl(unsafeFile);
      const reader = new FileReader();
      reader.readAsDataURL($event);

      database = reader.onload = () => {
        resolve({
          base: reader.result
        });
      };

      database = reader.onerror = () => {
        resolve({
          base: null
        });
      };
    } catch (error) {
      database = null
    }

    return database;
  })

  async guardar(){
    let xpta: string = await this.obtenerDatos();
    console.log("factura", this.pedido);    

    const txtSerie = document.getElementById("txtSerie") as HTMLInputElement
    const txtNumer = document.getElementById("txtNumero") as HTMLInputElement
    const txtFecha = document.getElementById("txtFechaDoc") as HTMLInputElement
    const txtTotal = document.getElementById("txtTotal") as HTMLInputElement

    const fd = txtFecha.value;

    // console.log("fecha", fd);
    
    let dataSunat: any = { }
    dataSunat.numRuc = this.pedido.licTradNum;
    dataSunat.codComp = "01";
    dataSunat.numeroSerie = txtSerie.value;
    dataSunat.numero = txtNumer.value;
    dataSunat.fechaEmision = fd;
    dataSunat.monto = txtTotal.value.replace(",","")
    
    if(xpta != ""){
      this.msgError = xpta.split("\n").join("<br />");
      this.errorValidacion = true;
      return;
    }

    // const dataToken: any = await this.documentoService.obtenerTokenSunat().toPromise();
    // console.log(dataToken);
    // console.log(dataSunat);
    
    // if(dataToken.access_token){
    //   const dataFactura: any = await this.documentoService.validarFactura(dataSunat, dataToken.access_token).toPromise();
    //   switch(dataFactura.data.estadoCp){
    //     case "0":
    //       this.msgError = "Factura no existe en SUNAT"
    //       break;
    //     case "1":
    //       this.msgError = "Aceptada"
    //       break;
    //     case "2":
    //       this.msgError = "Factura anulada"
    //       break;
    //     case "3":
    //       this.msgError = "Autorizada"
    //       break;
    //     case "4":
    //       this.msgError = "Factura no autorizada"
    //       break;
    //   }

    //   if(dataFactura.data.estadoCp != "1" && dataFactura.data.estadoCp != "3"){
    //     this.errorValidacion = true;
    //     return;
    //   } 
    // }

    this.flagLoad = true;
    this.pedido.paidToDate = this.pedido.paidToDate.replace(",", "")

    try {
      await this.documentoService.crearFacturaAnticipo(this.pedido).toPromise().then(data => {
        this.msgError = data?.toString().split("\n").join("<br />");
        console.log("Respuesta", data);
        this.errorValidacion = false;
        // console.log(this.msgError);
        if(this.msgError?.includes("exitó"))
          this.flagExito = true;
        else
          this.flagError = true;
  
      this.flagLoad = false;
      });
    } catch (error: any) {
      this.flagLoad = false;
      this.flagError = true;
      this.msgError = error.toString()
    }   
  }

  async obtenerDatos(): Promise<string>{
    let xVal: string = "";
    try {
      
      if(this.nomArchivo1 == ""){
        xVal = "Tiene que adjuntar el XML";        
      }        

      if(this.nomArchivo2 == ""){
        xVal = "Tiene que adjuntar el CDR";        
      }    

      if(this.nomArchivo3 == ""){
        xVal = "Tiene que adjuntar el PDF";        
      }    

      // if(this.nomArchivo4 == ""){
      //   xVal = "Tiene que adjuntar la Garantia por cumplimiento";        
      // }    

      this.pedido.userReg = this.userProv;

      this.pedido.archivo = this.archivo1;
      this.pedido.nomArchivo = this.nomArchivo1;

      this.pedido.archivo2 = this.archivo2;
      this.pedido.nomArchivo2 = this.nomArchivo1;

      this.pedido.archivo3 = this.archivo3;
      this.pedido.nomArchivo3 = this.nomArchivo3;

      if(this.archivo4 == undefined)
        this.archivo4 = null;

      if(this.archivo5 == undefined)
        this.archivo5 == null;

      this.pedido.archivo4 = this.archivo4;
      this.pedido.nomArchivo4 = this.nomArchivo4;

      this.pedido.archivo5 = this.archivo5;
      this.pedido.nomArchivo5 = this.nomArchivo5;  
      
      const xSerie = document.getElementById("txtSerie") as HTMLInputElement
      const xNumero = document.getElementById("txtNumero") as HTMLInputElement
      const txtFecha = document.getElementById("txtFechaDoc") as HTMLInputElement

      if(xSerie.value == ""){
        xVal = "Tiene que colocar una serie de documento";
      }
  
      if(xNumero.value == ""){
        xVal = "Tiene que colocar una número de documento";
      }
  
      let fecha: Date;
      if(txtFecha.value == "")
        xVal = "Falta Fecha de vencimiento"

      this.pedido.folioPref = xSerie.value;
      this.pedido.folioNum = xNumero.value;

      const xPorcentaje = document.getElementById("txtPorAnticipo") as HTMLInputElement
      if(xPorcentaje.value == "" || xPorcentaje.value == "0.00"){
        xVal = "El porcentaje debe ser diferente de 0";        
      } 
      
      const xImporte = document.getElementById("txtMonAnticipo") as HTMLInputElement
      
      this.pedido.paidToDate = xImporte.value;
      this.pedido.discountPercent = xPorcentaje.value

    } catch (error: any) {
      xVal = error.toString();
    }

    return xVal;
  }

  btnOk(){
    this.router.navigateByUrl('/factura', { replaceUrl: true });
  }

  openDocuments(){
    this.listaCS = [];
    this.modalDialog = true;
  }

  calcularImpAnticipo(tipo: string){
    const xPorcentaje = document.getElementById("txtPorAnticipo") as HTMLInputElement
    const xImporte = document.getElementById("txtMonAnticipo") as HTMLInputElement
    const txtSubTotal = document.getElementById("txtSubTotal") as HTMLInputElement

    if(tipo == "1"){
      const impAnticipo: number = Number(txtSubTotal.value.replace(",","")) * Number(xPorcentaje.value.replace(",","")) / 100;
      xImporte.value = new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(impAnticipo), ); 
      xPorcentaje.value = new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(xPorcentaje.value.replace(",","")), );
    }

    if(tipo == "2"){
      const impAnticipo: number = Number(xImporte.value.replace(",","")) / Number(txtSubTotal.value.replace(",","")) * 100;
      xPorcentaje.value = new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(impAnticipo), );
      xImporte.value = new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(xImporte.value.replace(",","")), );
    }
  }

  leerXML(event: any){
    const txtSerie = document.getElementById("txtSerie") as HTMLInputElement
    const txtNumero = document.getElementById("txtNumero") as HTMLInputElement
    const txtFechaDoc = document.getElementById("txtFechaDoc") as HTMLInputElement
    const reader = new FileReader();
    reader.onload = (e: any) => {
      console.log(e);
      
      let xml = e.target.result;
      this.inputXml = xml;
      let result1 = converter.xml2json(xml, {compact: true, spaces: 2});

      const JSONData = JSON.parse(result1);
      const { Invoice } = JSONData
      // console.log("Invoice", Invoice);
      let prop = ""
      prop = "cbc:ID"
      const { [prop]: serieNumero } = Invoice
      // console.log("Serie y Numero", serieNumero);
      const valSerie = serieNumero._text.split("-")
      txtSerie.value = valSerie[0];
      txtNumero.value = valSerie[1];

      prop = "cac:TaxTotal"
      const { [prop]: monedaImporte } = Invoice
      // console.log("valores", monedaImporte);
      prop = "cbc:IssueDate"
      const { [prop]: fecha } = Invoice
      // console.log("fecha", fecha);
      txtFechaDoc.value = fecha._text;
		}
    reader.readAsText(event.target.files[0]);

    const fileCopy = event.target.files[0];
    this.extraerBase64(fileCopy).then((file: any) => {
      this.archivo1 = (file.base).split(',')[1];
      this.nomArchivo1 = fileCopy.name;
      this.msgError = "";
      this.errorValidacion = false;
    })
  }
}
