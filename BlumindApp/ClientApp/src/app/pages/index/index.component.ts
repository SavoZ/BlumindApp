import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_ENDPOINT } from 'src/app/_constants/url.constants';
import { GroupDescriptor, DataResult, process } from '@progress/kendo-data-query';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {

  constructor(private http: HttpClient) {
    this.products = this.http.get(API_ENDPOINT + 'Product/GetProducts').toPromise();
  }
  products;
  public groups: GroupDescriptor[] = [{ field: 'category' }];
  public gridView: DataResult;

  ngOnInit() {
    this.loadProducts();

  }

  public groupChange(groups: GroupDescriptor[]): void {
    this.groups = groups;
    this.loadProducts();
  }

  private loadProducts(): void {
    this.gridView = process(this.products, { group: this.groups });
  }

}
