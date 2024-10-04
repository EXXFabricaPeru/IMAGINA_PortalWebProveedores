import { Component } from '@angular/core';
import { TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, AlertComponent, ContainerComponent, RowComponent, ColComponent, CardGroupComponent, 
  FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective,
  TableModule, UtilitiesModule } from '@coreui/angular';
import { NgStyle, CommonModule } from '@angular/common';
import { IconDirective } from '@coreui/icons-angular';
import { ActivatedRoute, Router } from '@angular/router';
import { DocumentoService } from 'src/app/services/documento.service';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-conformidad-servicio-crear',
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
  templateUrl: './conformidad-servicio-crear.component.html',
  styleUrl: './conformidad-servicio-crear.component.scss'
})
export class ConformidadServicioCrearComponent {
  errorValidacion: Boolean = false;
  flagExito: Boolean = false;
  flagError: Boolean = false;
  confirmDialog: Boolean = false;
  flagLoad: Boolean = false;

  msgError?: string = "";
  listaHeader: any[] = [];
  _rucProveedor: string = "";
  listaPedidos: any = [];
  detalle: any;
  pagina: number = 1;
  pedido: any = {docStatus: ""};
  archivo: any;
  nomArchivo: string = "";
  archivo1: any;
  nomArchivo1: string = "";
  archivo2: any;
  nomArchivo2: string = "";
  userProv: string = "";
  tipoPedido: string = "";
  decimalPrec: number = 0;
  decimalCant: number = 0;

  constructor(private _route: ActivatedRoute, private router: Router, private documentoService: DocumentoService, private sanitizer: DomSanitizer){
    // const dataTemp: any = localStorage.getItem("prov");
    const dataTemp: any = sessionStorage.getItem("prov");
    const dataProv: any = JSON.parse(dataTemp);
    // console.log("prov", dataProv)
    this._rucProveedor = dataProv.licTradNum;
    this.userProv = dataProv.licTradNum;

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
        label: "Cant. Pend.",
        key: "cantidad",
        subKey: "",
        customClass: "derecha",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "Cant. Aten.",
        key: "pendQuantity",
        subKey: "",
        customClass: "derecha",
        type: "text",
        value: "",
        visible: true
      },
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
      {
        label: "",
        key: "",
        subKey: "",
        customClass: "btnEdit",
        type: "buttonDel",
        value: "",
        visible: true
      }
    ];
    
    let docEntry: string | null;

    docEntry = this._route.snapshot.paramMap.get("id") || "";

    this.obtenerPedido(docEntry);
  }

  async obtenerPedido(id: string){
    const dataTempC: any = sessionStorage.getItem("config");
    const dataConfig: any = JSON.parse(dataTempC);

    for(let i=0; i<dataConfig.length; i++){
      if(dataConfig[i].descripcion == "PrecioDec")
        this.decimalPrec = dataConfig[i].valor_01

      if(dataConfig[i].descripcion == "CantidadDec")
        this.decimalCant = dataConfig[i].valor_01
    }

    // console.log(this.decimalCant, this.decimalPrec);
    
    try {      
      await this.documentoService.getPedidoId(id).toPromise().then(data => {
        console.log("pedido", data);
        this.pedido = data;
        this.tipoPedido = this.pedido.tipoDocumento;
        this.listaPedidos = this.pedido.detallePedido;

        this.pedido.subTotal = new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(this.pedido.docTotal) - Number(this.pedido.vatSum), );
        this.pedido.docTotal = (new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(this.pedido.docTotal), )).toString();
        this.pedido.vatSum = new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(this.pedido.vatSum), );

        this.listaPedidos.forEach((item: any) => {
          const index: number = this.listaPedidos.indexOf(item);
          // console.log(`Element at index ${index}: ${item}`);
          if (index !== -1 && item.cantidad == 0) {
              this.listaPedidos.splice(index, 1);
          }
        });

        for(let i=0; i<this.listaPedidos.length; i++){
          this.listaPedidos[i].price = (new Intl.NumberFormat("es-PE", { minimumFractionDigits: this.decimalPrec }).format(Number(this.listaPedidos[i].price), )).toString();
          this.listaPedidos[i].lineTotal = (new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(this.listaPedidos[i].lineTotal), )).toString();
          this.listaPedidos[i].cantidad = (new Intl.NumberFormat("es-PE", { minimumFractionDigits: this.decimalCant }).format(Number(this.listaPedidos[i].cantidad), )).toString();
          this.listaPedidos[i].pendQuantity = (new Intl.NumberFormat("es-PE", { minimumFractionDigits: this.decimalCant }).format(Number(this.listaPedidos[i].pendQuantity), )).toString();
        }
      });      
    } catch (error) {
      
    }
  }

  capturarFile(event: any, tipo: string){
    const fileCopy = event.target.files[0];
    // console.log(fileCopy);
    
    this.extraerBase64(fileCopy).then((file: any) => {
      // console.log(file);
      switch(tipo){
        case "1":
          this.archivo = (file.base).split(',')[1];
          this.nomArchivo = fileCopy.name;
          break;
        case "2":
          this.archivo1 = (file.base).split(',')[1];
          this.nomArchivo1 = fileCopy.name;
          break;
        case "3":
          this.archivo2 = (file.base).split(',')[1];
          this.nomArchivo2 = fileCopy.name;
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
    const xpta: string = await this.obtenerDatos();

    if(xpta != ""){
      this.msgError = xpta;
      this.errorValidacion = true;
      return;
    }
    // console.log("CS", this.pedido);
    try {
      this.flagLoad = true;
      await this.documentoService.confirmarOC(this.pedido).toPromise().then(dataGuardar => {
        console.log("mensje guardar", dataGuardar);

        this.flagLoad = false;

        this.msgError = dataGuardar;
        this.flagExito = true;
      });
      
    } catch (error: any) {
      this.flagLoad = false;
      console.log("error", error);
      this.flagError = true;
      this.msgError = error.message.toString();
    }
  }

  async obtenerDatos(): Promise<string>{
    let xVal: string = "";
    try {
      
      if(this.nomArchivo == ""){
        xVal = "Tiene que adjuntar un archivo";        
      }        

      const _table: any = document.querySelector("#gridDoc");
      // console.log(_table);
      
      for(let i = 1; i < _table.rows.length; i++){
        let cantidad: number = 0;
        let importe: number = 0;

        if(this.tipoPedido == "I"){
          const _cant: string = _table.rows[i].cells[2].children[0].value;
          cantidad = Number(_cant.replace(",", ""));
          
          if(cantidad == 0){
            xVal += `\n - Debe colocar una cantidad en la fila ${ i }`
          }

          // console.log(this.listaPedidos[i - 1].pendQuantity, _cant);
          if(this.listaPedidos[i - 1].pendQuantity < cantidad){
            xVal += `\n La cantidad atendida no puede puede ser mayor a la cantidad disponible en la fila ${ i }.`;
          }
          this.listaPedidos[i - 1].cantidad = cantidad;
        }
        else{
          // console.log(_table.rows[i].cells[7].children[0].value);
          
          importe = Number(_table.rows[i].cells[7].children[0].value);
          if(this.listaPedidos[i - 1].price < importe){
            xVal += `\n El importe no puede puede ser mayor al importe pendiente en la fila  ${ i }.`;
          }
          this.listaPedidos[i - 1].importe = importe;
        }

        this.listaPedidos[i - 1].lineTotal = this.listaPedidos[i - 1].lineTotal.toString().replace(",", "");
        this.listaPedidos[i - 1].pendQuantity = this.listaPedidos[i - 1].pendQuantity.toString().replace(",", "");
        this.listaPedidos[i - 1].price = this.listaPedidos[i - 1].price.toString().replace(",", "");
      }

      const txtComentario = document.getElementById("txtComentario") as HTMLInputElement;

      this.pedido.userReg = this.userProv;
      this.pedido.archivo = this.archivo
      this.pedido.nomArchivo = this.nomArchivo;
      this.pedido.archivo2 = this.archivo1
      this.pedido.nomArchivo2 = this.nomArchivo1;
      this.pedido.archivo3 = this.archivo2
      this.pedido.nomArchivo3 = this.nomArchivo2;
      this.pedido.docTotal = this.pedido.docTotal.toString().replace(",", "");
      this.pedido.vatSum = this.pedido.vatSum.toString().replace(",", "");
      this.pedido.comments = txtComentario.value;
      
    } catch (error: any) {
      xVal = error.toString();
    }

    return xVal;
  }

  async deleteRow(row: any){
    this.detalle = row;
    this.confirmDialog = true;
  }

  deleteProduct(){
    if(this.listaPedidos.length > 1){
      const index: number = this.listaPedidos.indexOf(this.detalle);
      if (index !== -1) {
          this.listaPedidos.splice(index, 1);
      }
    }else{
      this.errorValidacion = true;
      this.msgError = "No se puede eliminar tiene que haber al menos una línea";
    }
    this.confirmDialog = false;
  }

  btnOk(){
    this.router.navigateByUrl('/conformidad-servicio', { replaceUrl: true });
  }

  actualizarCant( row: any){    
    const _table: any = document.querySelector("#gridDoc");
    let _subTotal: number = 0;
    let _impuesto: number = 0;
    let _docTotal: number = 0;
    let _docTotalSD: number = 0;

    if(_table.rows.length - 1 == this.listaPedidos.length){
      // console.log("<-aqui->");
      for(let i = 1; i < _table.rows.length; i++){
        const xCantidad: number = Number(this.listaPedidos[i-1].cantidad);
        const xCant: number = Number(_table.rows[i].cells[2].children[0].value);
        // console.log("xxCant", xCant);
        const xPrec: number = Number(this.listaPedidos[i-1].price.toString().replace(",", ""));
        // console.log("xPrec", xPrec);
        
        if(xCantidad < xCant){
          this.msgError = "La cantidad no puede ser mayor a la disponible";
          this.errorValidacion = true;
          this.listaPedidos[i-1].pendQuantity = this.listaPedidos[i-1].cantidad;
          return;
        }else{
          this.errorValidacion = false;
        }

        const xDesc: number = 1 - Number(this.listaPedidos[i-1].discountPercent)//(Number(this.listaPedidos[i-1].descuento) / 100)
        // console.log("Descuento", xDesc);
        
        let xPrecioTot: number = Number((Number(xCant) * Number(xPrec) * Number(xDesc)).toFixed(2));
        // console.log("Precio Total", xPrecioTot);

        let xPreSinDes: number = Number((Number(xCant) * Number(xPrec)).toFixed(2));
        // console.log("Precio Sin", xPreSinDes);

        _docTotalSD = (_docTotalSD) + (xPreSinDes);
        // // console.log("Total", _docTotalSD);

        this.listaPedidos[i-1].lineTotal = xPrecioTot;
        this.listaPedidos[i-1].pendQuantity = xCant;

        _subTotal = _subTotal + xPrecioTot;
        console.log("Sub Total", _subTotal);

        if(this.listaPedidos[i-1].taxCode == "IGV"){
          _impuesto = _impuesto + Number((xPrecioTot* 0.18).toFixed(2));
          // console.log("Impuesto", _impuesto);
        }

        this.listaPedidos[i -1].lineTotal = new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(this.listaPedidos[i - 1].lineTotal), );
      }
      // console.log(this.listaPedidos);
       
      const descuento:number = 0;
      
      _docTotal = _subTotal + _impuesto - descuento

      this.pedido.subTotal = _subTotal;
      this.pedido.vatSum = _impuesto;
      this.pedido.docTotal = _docTotal;  
      
      this.pedido.subTotal = new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(this.pedido.subTotal), );
      this.pedido.docTotal = new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(this.pedido.docTotal), );
      this.pedido.vatSum = new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(this.pedido.vatSum), );

    }else{
      // console.log("aqui->");
      let stot: number = 0;
      for(let i = 0; i < this.listaPedidos.length; i++){
        stot += Number(this.listaPedidos[i].lineTotal);
        this.listaPedidos[i].lineTotal = new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(this.listaPedidos[i].lineTotal), );
      }

      this.pedido.subTotal = stot;
      this.pedido.vatSum = (stot * 0.18);
      this.pedido.docTotal = (stot * 1.18);
      
      this.pedido.subTotal = new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(this.pedido.subTotal), );
      this.pedido.docTotal = new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(this.pedido.docTotal), );
      this.pedido.vatSum = new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(this.pedido.vatSum), );
    }

  }

}
  