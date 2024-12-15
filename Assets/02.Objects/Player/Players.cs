using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Players : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    public FixedJoystick joy;
    public float speed;

    Rigidbody rigid;
    Animator anim;
    Vector3 moveVec;
    [SerializeField] Transform mainCanvas;

    public int accessory = -1;
    bool isOnCloset = false; 
    bool isOnBooth = false; 
    bool isOnTreasure = false;
    bool isSitting = false;
    bool isDancing = false;
    bool isTalking = false;
    [SerializeField] GameObject currentObj = null;
    Coroutine currentCo = null;
    public int NPCstatus = -1; //-1: 얘기 안함, 0: main이랑 처음 대화 - 선택지 3개 보여주기, 1: main이랑 첫번째 선택지 대화 - 다음 보여주기, 2: main 2번째 선택지, 3: sub랑 첫 대화

    public Transform cameraTransform; // 메인 카메라 참조
    [SerializeField] Animator BoothAnimator;
    [SerializeField] Sprite[] BoothSprites;
    [SerializeField] GameObject danceManager; 

    [SerializeField] PortalInfo[] portals;

    void Awake(){
        rigid = GetComponent<Rigidbody>();
        anim  = GetComponent<Animator>();
    }

    void FixedUpdate(){

        float x = joy.Horizontal;
        float z = joy.Vertical;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        moveVec = (camForward * z + camRight * x).normalized * speed * Time.fixedDeltaTime;
        if (currentCo == null && !isSitting && !isTalking){
            if(moveVec.sqrMagnitude > 0){
                rigid.MovePosition(rigid.position + moveVec);
                Quaternion dirQuat = Quaternion.LookRotation(moveVec);
                rigid.rotation = Quaternion.Slerp(rigid.rotation, dirQuat, 0.2f);
                anim.SetBool("isWalk", true);
            }
            else {
                anim.SetBool("isWalk", false);
            }
        }
    }
    public void ClosetBtn(){
        if (isOnCloset){
            if (mainCanvas.GetChild(2).gameObject.activeSelf){ //옷장 끄기
                mainCanvas.GetChild(2).gameObject.SetActive(false);
                gameObject.transform.GetChild(3).gameObject.SetActive(false);
                cameraTransform.gameObject.SetActive(true);
            }
            else { //옷장 켜기
                mainCanvas.GetChild(2).gameObject.SetActive(true);
                gameObject.transform.GetChild(3).gameObject.SetActive(true);
                cameraTransform.gameObject.SetActive(false);
            }
        }
    }

    public void BoothBtn(){
        if (isOnBooth && (currentCo == null)){
            currentCo = StartCoroutine(IntoBooth());
        }
    }

    public void TreasureBtn(){
        if (isOnTreasure && currentObj != null){
            currentCo = StartCoroutine(TreasureOpen());
        }
    }

    public void DanceBtn(){
        if (!isDancing){
            StartCoroutine(StartDance());
        }
    }

    public void PortalBtn(){
        if (currentObj.GetComponent<PortalInfo>().isPortalEntrance){
            transform.position = portals[1].transform.position - new Vector3(0, 0, 1);
            transform.rotation = portals[1].transform.rotation;

        }
        else {
            transform.position = portals[0].transform.position + new Vector3(0, 0, 1);
            transform.rotation = portals[0].transform.rotation;
        }
    }

    public void NPCBtn(){ //처음 대화할때
        isTalking = true;
            mainCanvas.GetChild(3).GetChild(2).gameObject.SetActive(false); //상호작용 버튼 끄기
            if (currentObj.GetComponent<NPCInfo>().isWheel){ //sub랑 첫 대화
                NPCstatus = 3;
                mainCanvas.GetChild(6).GetChild(1).GetChild(0).GetComponent<Text>().text = "낑낑돌이";
                mainCanvas.GetChild(6).GetChild(2).gameObject.SetActive(true); //btncontainer키기
                mainCanvas.GetChild(6).GetChild(2).GetChild(1).gameObject.SetActive(true); //subbtn
                mainCanvas.GetChild(6).GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(true); //finbtn 키기
                mainCanvas.GetChild(6).GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(false); //nextbtn 끄기
            }
            else {
                NPCstatus = 0;
                mainCanvas.GetChild(6).GetChild(1).GetChild(0).GetComponent<Text>().text = "버섯돌이";
                mainCanvas.GetChild(6).GetChild(2).gameObject.SetActive(true); //btncontainer 키기
                mainCanvas.GetChild(6).GetChild(2).GetChild(1).gameObject.SetActive(false); //sub container 끄기
                mainCanvas.GetChild(6).GetChild(2).GetChild(0).gameObject.SetActive(true); //main container 키기
            } 
            currentObj.GetComponent<Animator>().SetTrigger("talk");
            mainCanvas.GetChild(6).GetChild(0).GetChild(0).GetComponent<Text>().text = currentObj.GetComponent<NPCInfo>().chat1;
            mainCanvas.GetChild(6).gameObject.SetActive(true);
    }
    public void NextBtn(){
        NPCstatus = 0;
        mainCanvas.GetChild(6).GetChild(1).GetChild(0).GetComponent<Text>().text = "버섯돌이";
        mainCanvas.GetChild(6).GetChild(2).gameObject.SetActive(true); //btncontainer 키기
        mainCanvas.GetChild(6).GetChild(2).GetChild(1).gameObject.SetActive(false); //sub container 끄기
        mainCanvas.GetChild(6).GetChild(2).GetChild(0).gameObject.SetActive(true); //main container 키기
        mainCanvas.GetChild(6).GetChild(0).GetChild(0).GetComponent<Text>().text = currentObj.GetComponent<NPCInfo>().chat1;
    }

    public void FinChatBtn(){
        isTalking = false;
        currentObj.GetComponent<Animator>().SetTrigger("walk");
        mainCanvas.GetChild(6).gameObject.SetActive(false);
        mainCanvas.GetChild(6).GetChild(2).gameObject.SetActive(false);
        mainCanvas.GetChild(6).GetChild(2).GetChild(0).gameObject.SetActive(false); //main container
        mainCanvas.GetChild(6).GetChild(2).GetChild(1).gameObject.SetActive(false); //sub container
        mainCanvas.GetChild(6).GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(false); //finbtn 끄기
        mainCanvas.GetChild(6).GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(false); //nextbtn 끄기
        NPCstatus = -1;
    }

    public void MainFirstBtn(){ 
        mainCanvas.GetChild(6).GetChild(0).GetChild(0).GetComponent<Text>().text = currentObj.GetComponent<NPCInfo>().chat2; //대화 내용
        mainCanvas.transform.GetChild(6).GetChild(2).GetChild(0).gameObject.SetActive(false); //main끄기
        mainCanvas.transform.GetChild(6).GetChild(2).GetChild(1).gameObject.SetActive(true); //SUB키기
        mainCanvas.transform.GetChild(6).GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(true); //next키기
        mainCanvas.transform.GetChild(6).GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(false); //fin끄기
        NPCstatus = 1;
    }
    public void MainSecondBtn(){ 
        mainCanvas.GetChild(6).GetChild(0).GetChild(0).GetComponent<Text>().text = currentObj.GetComponent<NPCInfo>().chat3; //대화 내용
        mainCanvas.transform.GetChild(6).GetChild(2).GetChild(0).gameObject.SetActive(false); //main끄기
        mainCanvas.transform.GetChild(6).GetChild(2).GetChild(1).gameObject.SetActive(true); //SUB키기
        mainCanvas.transform.GetChild(6).GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(true); //next키기
        mainCanvas.transform.GetChild(6).GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(false); //fin끄기
        NPCstatus = 2;
    }
    public void MainFinBtn(){
        isTalking = false;
        currentObj.GetComponent<Animator>().SetTrigger("walk");
        mainCanvas.GetChild(6).gameObject.SetActive(false);
        mainCanvas.GetChild(6).GetChild(2).gameObject.SetActive(false);
        mainCanvas.GetChild(6).GetChild(2).GetChild(0).gameObject.SetActive(false); //main container
        mainCanvas.GetChild(6).GetChild(2).GetChild(1).gameObject.SetActive(false); //sub container
        mainCanvas.GetChild(6).GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(false); //finbtn 끄기
        mainCanvas.GetChild(6).GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(false); //nextbtn 끄기
        mainCanvas.transform.GetChild(6).GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(false); //next키기
        mainCanvas.transform.GetChild(6).GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(false); //fin끄기
        NPCstatus = -1;
    }

    IEnumerator StartDance(){
        isDancing = true;
        mainCanvas.GetChild(3).GetChild(5).gameObject.SetActive(false); 
        danceManager.SetActive(true);
        audioManager.transform.GetChild(0).GetComponent<AudioSource>().volume = 0f;
        audioManager.PlayEffect("dance");
        yield return new WaitForSeconds(24f);
        audioManager.transform.GetChild(0).GetComponent<AudioSource>().volume = 1f;
        isDancing = false;
        if(currentObj!=null && currentObj.CompareTag("Dance")) mainCanvas.GetChild(3).GetChild(6).gameObject.SetActive(true); 
    }
    
    IEnumerator TreasureOpen(){
        currentObj.transform.GetChild(1).gameObject.SetActive(true);
        currentObj.transform.GetChild(0).gameObject.SetActive(false);
        mainCanvas.GetChild(3).GetChild(3).gameObject.SetActive(false); 
        isOnTreasure = false;
        yield return(StartCoroutine(UsingLerp(currentObj.transform, currentObj.transform.localPosition + new Vector3(0, 1, 0), currentObj.transform.localEulerAngles, 1f)));
        mainCanvas.GetChild(2).GetChild(currentObj.GetComponent<TreasureInfo>().intTreasure).gameObject.SetActive(true);
        Destroy(currentObj.gameObject);
        currentObj = null;
        mainCanvas.GetChild(5).gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        mainCanvas.GetChild(5).gameObject.SetActive(false);
        currentCo = null;
    }

    IEnumerator IntoBooth(){
        GameObject photoLight = BoothAnimator.gameObject.transform.parent.GetChild(8).gameObject;
        mainCanvas.GetChild(3).GetChild(1).gameObject.SetActive(false); //p를 눌러 텍스트
        BoothAnimator.transform.parent.GetComponent<BoxCollider>().isTrigger = true;
        anim.SetBool("isWalk", true);
        yield return(StartCoroutine(UsingLerp(transform, new Vector3(19.3665447f,1.89726496f,13.4565783f), new Vector3(0,45,0), 0.5f)));
        yield return(StartCoroutine(UsingLerp(transform, new Vector3(21.59f,1.87570262f,15.3f), new Vector3(0, 60, 0), 0.5f)));
        anim.SetBool("isWalk", false);
        BoothAnimator.SetTrigger("Close");
        yield return new WaitForSeconds(1f);
        transform.localEulerAngles = new Vector3(0, 240, 0);
        photoLight.SetActive(true);
        audioManager.PlayEffect("cam");
        yield return new WaitForSeconds(0.1f);
        photoLight.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        photoLight.SetActive(true);
        audioManager.PlayEffect("cam");
        yield return new WaitForSeconds(0.1f);
        photoLight.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        photoLight.SetActive(true);
        audioManager.PlayEffect("cam");
        yield return new WaitForSeconds(0.1f);
        photoLight.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        photoLight.SetActive(true);
        audioManager.PlayEffect("cam");
        yield return new WaitForSeconds(0.1f);
        photoLight.SetActive(false);
        BoothAnimator.SetTrigger("Open");
        yield return new WaitForSeconds(1f);
        anim.SetBool("isWalk", true);
        yield return(StartCoroutine(UsingLerp(transform, new Vector3(19.3665447f,1.89726496f,13.4565783f), new Vector3(0,240,0), 0.5f)));
        anim.SetBool("isWalk", false);
        BoothAnimator.transform.parent.GetComponent<BoxCollider>().isTrigger = false;
        mainCanvas.GetChild(4).GetChild(0).localPosition = new Vector3(595,1210,0);
        mainCanvas.GetChild(4).GetChild(0).GetComponent<Image>().sprite = BoothSprites[accessory+1];
        mainCanvas.GetChild(4).gameObject.SetActive(true);
        yield return(StartCoroutine(UsingLerp(mainCanvas.GetChild(4).GetChild(0), new Vector3(595,-8,0), Vector3.zero, 0.5f)));
        yield return new WaitForSeconds(2f);
        mainCanvas.GetChild(4).gameObject.SetActive(false);
        mainCanvas.GetChild(3).GetChild(1).gameObject.SetActive(true); //p를 눌러 텍스트
        currentCo = null;
    }

    public void SitBtn(){
        if (!isSitting && currentObj.CompareTag("Sit")){
            anim.SetTrigger("Sit");
            isSitting = true;
            transform.position = currentObj.transform.GetChild(1).position;
            transform.eulerAngles = currentObj.transform.GetChild(1).eulerAngles;
            GetComponent<CapsuleCollider>().isTrigger = true;
            rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            transform.parent = currentObj.transform;
        }
        else if (isSitting && currentObj.CompareTag("Sit")){
            anim.SetTrigger("Stand");
            isSitting = false;
            transform.position = currentObj.transform.GetChild(2).position;
            GetComponent<CapsuleCollider>().isTrigger = false;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            transform.parent = mainCanvas.transform.parent;
        }
    }

    IEnumerator UsingLerp(Transform obj, Vector3 destPos, Vector3 destRot, float destTime)
    {
    WaitForSeconds wtf = new WaitForSeconds(Time.deltaTime / 2);
    float pastTime = 0f;
    Vector3 originPos = obj.localPosition;
    Vector3 originRot = obj.localEulerAngles;
    Vector3 originScale = obj.localScale;
    while (pastTime < destTime)
    {
        obj.localPosition = Vector3.Lerp(originPos, destPos, pastTime / destTime);
        obj.localEulerAngles = Vector3.Lerp(originRot, destRot, pastTime / destTime);
        pastTime += Time.deltaTime / 2;
        yield return wtf;
    }
    obj.localPosition = destPos;
    obj.localEulerAngles = destRot;
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Closet")){
            isOnCloset = true; 
            mainCanvas.GetChild(3).GetChild(0).gameObject.SetActive(true); //상호작용버튼
        }
        else if (other.CompareTag("Booth")){
            isOnBooth = true;
            mainCanvas.GetChild(3).GetChild(1).gameObject.SetActive(true); //상호작용버튼
        }
        else if (other.CompareTag("NPC")){
            mainCanvas.GetChild(3).GetChild(2).gameObject.SetActive(true); //상호작용버튼
            currentObj = other.gameObject;
        }
        else if (other.CompareTag("Treasure")){
            isOnTreasure = true;
            mainCanvas.GetChild(3).GetChild(3).gameObject.SetActive(true); //상호작용버튼
            currentObj = other.gameObject;
        }
        else if (other.CompareTag("Sit")){
            mainCanvas.GetChild(3).GetChild(4).gameObject.SetActive(true); //상호작용버튼
            currentObj = other.gameObject;
        }
        else if (other.CompareTag("Dance")){
            mainCanvas.GetChild(3).GetChild(5).gameObject.SetActive(true); //상호작용버튼
            currentObj = other.gameObject;
        }   
        else if (other.CompareTag("Screen")){
            mainCanvas.GetChild(3).GetChild(6).gameObject.SetActive(true);
        }
        else if (other.CompareTag("Portal")){
            mainCanvas.GetChild(3).GetChild(7).gameObject.SetActive(true); //상호작용버튼
            currentObj = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Closet")){
            isOnCloset = false;
            mainCanvas.GetChild(3).GetChild(0).gameObject.SetActive(false); //텍스트 끄기
            mainCanvas.GetChild(2).gameObject.SetActive(false); //옷장 끄기
            transform.GetChild(3).gameObject.SetActive(false); 
            cameraTransform.gameObject.SetActive(true); //카메라 키기
        }
        else if (other.CompareTag("Booth")){
            isOnBooth = false;
            mainCanvas.GetChild(3).GetChild(1).gameObject.SetActive(false); //텍스트 끄기
        }
        else if (other.CompareTag("NPC")){
            mainCanvas.GetChild(3).GetChild(2).gameObject.SetActive(false); //상호작용버튼
            currentObj = null;
        }
        else if (other.CompareTag("Treasure")){
            isOnTreasure = false;
            mainCanvas.GetChild(3).GetChild(3).gameObject.SetActive(false); 
            currentObj = null;
        }
        else if (other.CompareTag("Sit")){
            mainCanvas.GetChild(3).GetChild(4).gameObject.SetActive(false); //상호작용버튼
            currentObj = null;
        }
        else if (other.CompareTag("Dance")){
            mainCanvas.GetChild(3).GetChild(5).gameObject.SetActive(false); //상호작용버튼
            currentObj = null;
        }   
        else if (other.CompareTag("Screen")){
            mainCanvas.GetChild(3).GetChild(6).gameObject.SetActive(false);
        }
        else if (other.CompareTag("Portal")){
            mainCanvas.GetChild(3).GetChild(7).gameObject.SetActive(false); //상호작용버튼
            currentObj = null;
        }
    }

}
