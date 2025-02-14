import { User } from "./user.model"
import { Car } from "./car.model"

export interface UserCar {
    user: User,
    car: Car
}