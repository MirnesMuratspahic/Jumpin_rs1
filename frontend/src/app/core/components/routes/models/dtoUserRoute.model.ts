import { Coordinates } from "./coordinates.model";

export interface dtoUserRoute
{
  email: string;
  name: string;
  seatsNumber: number;
  coordinates: Coordinates[];
  dateAndTime: string;
  price: number;
  description: string;
  type: string;
}