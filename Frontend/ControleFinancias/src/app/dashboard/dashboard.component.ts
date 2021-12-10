import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Chart } from 'chart.js';




@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  @ViewChild("CanvasVendas", { static: true }) elemento: ElementRef;
  @ViewChild("CanvasEquipamento", { static: true }) elementoCircle: ElementRef;
  
  constructor() { }
  /*
  public barChartOptions = {
    scaleShowVerticalLines: false,
    responsive: true
  };
  public barChartLabels = ['2006', '2007', '2008', '2009', '2010', '2011', '2012'];
  public barChartType = 'bar';
  public barChartLegend = true;
  public barChartData = [
    {data: [65, 59, 80, 81, 56, 55, 40], label: 'Series A'},
    {data: [28, 48, 40, 19, 86, 27, 90], label: 'Series B'}
  ];
*/

  
  ngOnInit() {
    
    new Chart(this.elemento.nativeElement, {
      type: 'line',
      data: {
        labels: ["2010","2011","2012","2013","2014","2015","2016","2017","2018","2019","2020","2021"],
        datasets: [
          {
            data: [33,38,10,93,68,50,35,29,34,2,62,120],
            borderColor: "#FFCC00",
            fill: false
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
  }
}
