using UnityEngine;
public interface PowerUp
{
    //returns name of powerup
    string GetName();
    //returns description of powerup
    string GetDescription();
    // what to do when picked up

    /*for single-use items handled by PowerUpCollector and removed from pool in PowerUpPool
     * 
     */
    void OnCollect();
    // return true if the powerup can only be collected once (permanent effects, not reusable)
    bool isSingleUse();

    /*for multi-use items, handled by Inventory
     * 
     */
    // return true if the player can use item from inventory
    bool isUseable();
    // string representing which item this is
    string GetItem();
    // how many to add to inventory, only applies to useable items
    int GetAmount();
}
