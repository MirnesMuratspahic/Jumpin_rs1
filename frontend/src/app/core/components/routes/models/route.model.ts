import { Coordinates } from "./coordinates.model";

export interface Route {
  id: number;
  name: string;
  coordinates: Coordinates[];
  seatsNumber: number;
  dateAndTime: string;
  price: number;
  description: string;
  type: string;
}
