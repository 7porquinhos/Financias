import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Chart } from 'chart.js';
import { ToastrService } from 'ngx-toastr';
import { bufferToggle } from 'rxjs/operators';




@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  @ViewChild("CanvasVendas", { static: true }) elemento: ElementRef;
  @ViewChild("CanvasEquipamento", { static: true }) elementoCircle: ElementRef;
  @ViewChild("CanvasEquipamentos", { static: true }) elementoEquipamento: ElementRef;
  
  showNavDash: boolean;

  constructor( 
    public router: Router,
    private toastr: ToastrService) { }
  
  ngOnInit() {
    this.showNavDash = false;


    new Chart(this.elemento.nativeElement, {
      type: 'line',
      data: {
        labels: ["2010","2011","2012","2013","2014","2015","2016","2017","2018","2019","2020","2021"],
        datasets: [
          {
            label: 'Vendas Anual',
            data: [33,38,10,93,68,50,35,29,34,2,62,120],
            pointBackgroundColor: "#f29200",
            pointBorderColor: "white",
            backgroundColor: "rgba(215, 225, 236, 0.3)",
            borderColor: "white",
            fill: 'origin'
          }
        ]
      },
      options: {
        legend: {
          display: false
        }
      }
    });

    new Chart(this.elementoCircle.nativeElement, {
      type: 'doughnut',
      data: {
        labels: [
          'Cabine de Fotos',
          'Totem',
          'Espelho',
          'Lambe-Lambe'
        ],
        datasets: [
          {
            label: 'Equipamento + Vendido',
            data: [10, 20, 30,40],
            backgroundColor: [
              'rgb(255, 99, 132)',
              'rgb(54, 162, 235)',
              'rgb(255, 205, 86)',
              'rgb(147, 250, 165)'
            ]
          }]
      },
      options: {
        legend: {
          display: true
        }
      }
    });

    new Chart(this.elementoEquipamento.nativeElement, {
      type: 'horizontalBar',
      data: {
        labels: [
          'Cabine de Fotos',
          'Totem',
          'Espelho',
          'Lambe-Lambe'
        ],
        datasets: [
          {
            label: 'Equipamento + Vendido',
            data: [15, 20, 30,40],
            backgroundColor: [
              'rgb(255, 99, 132)',
              'rgb(54, 162, 235)',
              'rgb(255, 205, 86)',
              'rgb(147, 250, 165)'
            ]
          }]
      },
      options: {
        scales: {
          xAxes: [{
              ticks: {
                  min: 0 // Edit the value according to what you need
              }
          }],
          yAxes: [{
              stacked: true
          }]
      },
        legend: {
          display: true
        }
      }
    });
  }
  
  showMenuDash(chave: number){
    if(this.showNavDash == false && chave == 0)
    {
      this.showNavDash = true;
    }
    else{
      this.showNavDash = false;
    }
  }

  logout(){
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    this.toastr.show('Log Out');
    this.router.navigate(['/user/login']);
  }

}
