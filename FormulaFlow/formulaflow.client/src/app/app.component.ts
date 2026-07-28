import { HttpClient } from "@angular/common/http";
import { Component, OnInit, ChangeDetectionStrategy } from "@angular/core";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  standalone: false,
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: "./app.component.scss",
})
export class AppComponent {}
