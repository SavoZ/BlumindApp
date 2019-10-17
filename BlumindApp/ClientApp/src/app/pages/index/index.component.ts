import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { API_ENDPOINT } from 'src/app/_constants/url.constants';
import { GroupDescriptor, DataResult, process } from '@progress/kendo-data-query';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ProductEditModel } from 'src/app/_models/productEdit';
import { Router } from '@angular/router';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {

  constructor(private http: HttpClient, private router: Router) {
    this.GetProducts();
  }
  products = [];
  product = new ProductEditModel();
  public groups: GroupDescriptor[] = [{ field: 'category' }];
  public gridView: DataResult;
  public formGroup: FormGroup;
  private editedRowIndex: number;

  ngOnInit() {

  }

  private GetProducts() {
    this.http.get<any[]>(API_ENDPOINT + 'Product/GetProducts').toPromise().then(result => {
      this.products = result as [{}];
      this.loadProducts();

    });
  }

  public groupChange(groups: GroupDescriptor[]): void {
    this.groups = groups;
    this.loadProducts();
  }

  private loadProducts(): void {
    this.gridView = process(this.products, { group: this.groups });
  }

  public editHandler({ sender, rowIndex, dataItem }) {
    this.closeEditor(sender);

    this.router.navigate(['product/edit', dataItem.id]);
  }


  public removeHandler({ dataItem }) {
    this.http.delete(API_ENDPOINT + 'Product/DeleteProduct', { params: new HttpParams().set('productId', dataItem.id) })
      .toPromise().then(result => {
        this.GetProducts();
      });
  }

  public onClick({ dataItem }) {
    console.log(dataItem);
  }
  private closeEditor(grid, rowIndex = this.editedRowIndex) {
    grid.closeRow(rowIndex);
    this.editedRowIndex = undefined;
    this.formGroup = undefined;
  }

}
