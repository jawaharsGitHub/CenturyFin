import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { SafeResourceUrl } from '@angular/platform-browser/src/security/dom_sanitization_service';


@Component({
    templateUrl: './app/home/welcome.component.html'
})
export class WelcomeComponent  implements OnInit {
    
  name: string = "Hello";
  srcValue: SafeResourceUrl = './assets/files/tnDist1.svg';

  constructor(private domSanitizer: DomSanitizer) {
   }

  ngOnInit() {
    let URL = './assets/files/tnDist1.svg';
    this.srcValue = this.domSanitizer.bypassSecurityTrustResourceUrl(URL);
  }


  ShowDistrict()
  {
    let URL = './assets/files/dist.svg';
    this.srcValue = this.domSanitizer.bypassSecurityTrustResourceUrl(URL);
    //alert(this.srcValue);
  }

  ShowState()
  {
    let URL = './assets/files/tnDist1.svg';
    this.srcValue = this.domSanitizer.bypassSecurityTrustResourceUrl(URL);
  }
}
