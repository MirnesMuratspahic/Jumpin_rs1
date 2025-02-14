import { User } from "./user.model"
import { Flat } from "./flat.models"

export interface UserFlat {
    user: User,
    flat: Flat
}