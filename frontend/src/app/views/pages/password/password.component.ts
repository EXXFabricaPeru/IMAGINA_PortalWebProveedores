import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ProveedorService } from 'src/app/services/proveedor.service';
import { IconDirective } from '@coreui/icons-angular';
import { TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, AlertComponent, ContainerComponent, RowComponent, ColComponent, CardGroupComponent, 
  FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective,
  UtilitiesModule } from '@coreui/angular';
import { NgStyle, CommonModule } from '@angular/common';

@Component({
  selector: 'app-password',
  standalone: true,
  imports: [AlertComponent, ContainerComponent, RowComponent, ColComponent, CardGroupComponent, TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, UtilitiesModule, FormDirective, 
    InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective, NgStyle, CommonModule],
  templateUrl: './password.component.html',
  styleUrl: './password.component.scss'
})
export class PasswordComponent {
  errorValidacion: Boolean = false;
  flagExito: Boolean = false;
  msgError: string = "";
  
  constructor(private router: Router, private proveedorService: ProveedorService) {
    
  }

  async grabar(){
    const passOld: string = (document.getElementById("passOld") as HTMLInputElement).value;
    const passNew: string = (document.getElementById("passNew") as HTMLInputElement).value;
    const pasConf: string = (document.getElementById("passCon") as HTMLInputElement).value;

    if(passNew != pasConf){
      this.errorValidacion = true;
      this.msgError = "La nueva contraseña no coincide";
      return;
    }

    const data: any = {
      "code": "P20100067839",
      "password": passNew
    }

    const dataRpta: any = await this.proveedorService.changePassword(data).toPromise();

    if(dataRpta != "Nuevo Password"){
      this.errorValidacion = true;
      this.msgError = "Se produjo un error al actualizar la contraseña";
      return;
    }else{
      this.flagExito = true;
      this.msgError = "Se actualizó con éxito"
    }

  }
}
