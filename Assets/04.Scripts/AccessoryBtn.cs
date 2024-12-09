using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AccessoryBtn : MonoBehaviour
{
    [SerializeField] int index;
    BtnManager btnManager;
    void Awake(){
        btnManager = transform.parent.parent.GetComponent<BtnManager>();
    }
    public void BtnClick(){
        if (!btnManager.isAccessoryOn){
            btnManager.isAccessoryOn = true;
            btnManager.ownPlayerHead.GetChild(index+2).gameObject.SetActive(true);
            btnManager.transform.GetChild(index).GetChild(0).GetComponent<Image>().sprite = btnManager.onSprite;
            btnManager.intAccessory = index;
            GameObject.FindWithTag("Player").GetComponent<Players>().accessory = btnManager.intAccessory;
        }
        else {
            if (btnManager.intAccessory != index){
                btnManager.ownPlayerHead.GetChild(2+btnManager.intAccessory).gameObject.SetActive(false);
                btnManager.transform.GetChild(btnManager.intAccessory).GetChild(0).GetComponent<Image>().sprite = btnManager.offSprite;
                btnManager.intAccessory = index;
                btnManager.ownPlayerHead.GetChild(2+btnManager.intAccessory).gameObject.SetActive(true);
                btnManager.transform.GetChild(index).GetChild(0).GetComponent<Image>().sprite = btnManager.onSprite;
                GameObject.FindWithTag("Player").GetComponent<Players>().accessory = btnManager.intAccessory;
            }
            else {
                btnManager.ownPlayerHead.GetChild(2+btnManager.intAccessory).gameObject.SetActive(false);
                btnManager.transform.GetChild(btnManager.intAccessory).GetChild(0).GetComponent<Image>().sprite = btnManager.offSprite;
                btnManager.isAccessoryOn = false;
                btnManager.intAccessory = -1;
                GameObject.FindWithTag("Player").GetComponent<Players>().accessory = btnManager.intAccessory;
            }
        }
    }
}
