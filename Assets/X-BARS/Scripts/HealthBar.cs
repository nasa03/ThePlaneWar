using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//NOTE! You should hava a Camera with "MainCamera" tag and a canvas with a Screen Space - Overlay mode to script works properly;

public class HealthBar : MonoBehaviour {
	
	public RemoteIntHealth healthLink;				//A path to the Health filed;			
	public RectTransform HealthbarPrefab;			//Our health bar prefab;
	public float yOffset;							//Horizontal position offset;
	public bool keepSize = true;	                //keep distance independed size;
	public float scale = 1;							//Scale of the healthbar;
	public Vector2 sizeOffsets;						//Use this to overwright healthbar width and height values;
	public bool DrawOFFDistance;					//Disable health bar if it out of drawDistance;
	public float drawDistance = 10;
	public bool showHealthInfo;						//Show the health info on top of the health bar or not;
	public enum HealthInfoAlignment {top, center, bottom};
	public HealthInfoAlignment healthInfoAlignment = HealthInfoAlignment.center;
	public float healthInfoSize = 10;
    public AlphaSettings alphaSettings;
	private Image healthVolume, backGround;			//Health bar images, should be named as "Health" and "Background";
	private Text healthInfo;						//Health info, a healthbar's child Text object(should be named as HealthInfo);
	private CanvasGroup canvasGroup;
	private Vector2 healthbarPosition, healthbarSize, healthInfoPosition;
	private Transform thisT;
	private float defaultHealth, lastHealth, camDistance, dist, pos, rate;
	private Camera cam;
    private GameObject healthbarRoot;
	[HideInInspector]public Canvas canvas;
	
	void Awake()
	{
		Canvas[] canvases = (Canvas[])FindObjectsOfType(typeof(Canvas));
		if (canvases.Length == 0)
			Debug.LogError("There is no Canvas in the scene or Canvas GameObject isn't active, please create one - GameObject->UI->Canvas or activate existing");
		
		for (int i = 0; i < canvases.Length; i++)
		{
			if(canvases[i].enabled && canvases[i].gameObject.activeSelf && canvases[i].renderMode == RenderMode.ScreenSpaceOverlay)
				canvas = canvases[i];
			else
				Debug.LogError("There is no Canvas with RenderMode: ScreenSpace - Overlay in the scene or it's disabled, please create one - GameObject->UI->Canvas or enable existing");
		}

        defaultHealth = healthLink.Value;
        lastHealth = defaultHealth;
	}
	
	// Use this for initialization
	void Start () {
		if(!HealthbarPrefab)
		{
			Debug.LogWarning("HealthbarPrefab is empty, please assign your healthbar prefab in inspector");
			return;
		}
		
		thisT = this.transform;
        if (canvas.transform.Find("HealthbarRoot") != null)
            healthbarRoot = canvas.transform.Find("HealthbarRoot").gameObject;
        else
            healthbarRoot = new GameObject("HealthbarRoot", typeof(RectTransform), typeof(HealthbarRoot));
        healthbarRoot.transform.SetParent(canvas.transform, false);
		HealthbarPrefab = (RectTransform)Instantiate(HealthbarPrefab, new Vector2 (-1000, -1000), Quaternion.identity);
        HealthbarPrefab.name = "HealthBar";
        HealthbarPrefab.SetParent(healthbarRoot.transform, false);
		canvasGroup = HealthbarPrefab.GetComponent<CanvasGroup> ();
		
		healthVolume = HealthbarPrefab.transform.Find ("Health").GetComponent<Image>();
		backGround = HealthbarPrefab.transform.Find ("Background").GetComponent<Image>();
		healthInfo = HealthbarPrefab.transform.Find ("HealthInfo").GetComponent<Text> ();
		healthInfo.resizeTextForBestFit = true;
		healthInfo.rectTransform.anchoredPosition = Vector2.zero;
		healthInfoPosition = healthInfo.rectTransform.anchoredPosition;
		healthInfo.resizeTextMinSize = 1;
		healthInfo.resizeTextMaxSize = 500;
		
		healthbarSize = HealthbarPrefab.sizeDelta;
        canvasGroup.alpha = alphaSettings.fullAplpha;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!HealthbarPrefab)
			return;
		
		HealthbarPrefab.transform.position = cam.WorldToScreenPoint(new Vector3(thisT.position.x, thisT.position.y + yOffset, thisT.position.z));
		healthVolume.fillAmount =  healthLink.Value/defaultHealth;

		float maxDifference = 0.1F;


		if(backGround.fillAmount > healthVolume.fillAmount + maxDifference)
			backGround.fillAmount = healthVolume.fillAmount + maxDifference;
        if (backGround.fillAmount > healthVolume.fillAmount)
            backGround.fillAmount -= (1 / (defaultHealth / 100)) * Time.deltaTime;
        else
            backGround.fillAmount = healthVolume.fillAmount;
	}
	
	
	void Update()
	{
		if(!HealthbarPrefab)
			return;
		
		camDistance = Vector3.Dot(thisT.position - cam.transform.position, cam.transform.forward);
		
		if(showHealthInfo)
			healthInfo.text = healthLink.Value +" / "+defaultHealth;
		else
			healthInfo.text = "";

        if(lastHealth != healthLink.Value)
        {
            rate = Time.time + alphaSettings.onHit.duration;
            lastHealth = healthLink.Value;
        }

        if (!OutDistance() && IsVisible())
        {
            if (healthLink.Value <= 0)
            {
                if (alphaSettings.nullFadeSpeed > 0)
                {
                    if (backGround.fillAmount <= 0)
                        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, alphaSettings.nullAlpha, alphaSettings.nullFadeSpeed);
                }
                else
                    canvasGroup.alpha = alphaSettings.nullAlpha;
            }
            else if (healthLink.Value == defaultHealth)
                canvasGroup.alpha = alphaSettings.fullFadeSpeed > 0 ? Mathf.MoveTowards(canvasGroup.alpha, alphaSettings.fullAplpha, alphaSettings.fullFadeSpeed) : alphaSettings.fullAplpha;
            else
            {
                if (rate > Time.time)
                    canvasGroup.alpha = alphaSettings.onHit.onHitAlpha;
                else
                    canvasGroup.alpha = alphaSettings.onHit.fadeSpeed > 0 ? Mathf.MoveTowards(canvasGroup.alpha, alphaSettings.defaultAlpha, alphaSettings.onHit.fadeSpeed) : alphaSettings.defaultAlpha;
            }
        }
        else
            canvasGroup.alpha = alphaSettings.defaultFadeSpeed > 0 ? Mathf.MoveTowards(canvasGroup.alpha, 0, alphaSettings.defaultFadeSpeed) : 0;

		
		if(healthLink.Value <= 0)
			healthLink.Value = 0;

		dist = keepSize ? camDistance / scale :  scale;

		HealthbarPrefab.sizeDelta = new Vector2 (healthbarSize.x/(dist-sizeOffsets.x/100), healthbarSize.y/(dist-sizeOffsets.y/100));
		
		healthInfo.rectTransform.sizeDelta = new Vector2 (HealthbarPrefab.sizeDelta.x * healthInfoSize/10, 
		                                                  HealthbarPrefab.sizeDelta.y * healthInfoSize/10);
		
		healthInfoPosition.y = HealthbarPrefab.sizeDelta.y + (healthInfo.rectTransform.sizeDelta.y - HealthbarPrefab.sizeDelta.y) / 2;
		
		if(healthInfoAlignment == HealthInfoAlignment.top)
			healthInfo.rectTransform.anchoredPosition = healthInfoPosition;
		else if (healthInfoAlignment == HealthInfoAlignment.center)
			healthInfo.rectTransform.anchoredPosition = Vector2.zero;
		else
			healthInfo.rectTransform.anchoredPosition = -healthInfoPosition;

        if(healthLink.Value > defaultHealth)
            defaultHealth = healthLink.Value;
	}

	bool IsVisible()
	{
		return canvas.pixelRect.Contains (HealthbarPrefab.position);
	}

    bool OutDistance()
    {
        return DrawOFFDistance == true && camDistance > drawDistance;
    }

    public float GetDefaultHealth()
    {
        return defaultHealth;
    }

    public float GetCurrentHealth()
    {
        return healthLink.Value;
    }
}

[System.Serializable]
public class AlphaSettings
{
    
    public float defaultAlpha = 0.7F;           //Default healthbar alpha (health is bigger then zero and not full);
    public float defaultFadeSpeed = 0.1F;
    public float fullAplpha = 1.0F;             //Healthbar alpha when health is full;
    public float fullFadeSpeed = 0.1F;
    public float nullAlpha = 0.0F;              //Healthbar alpha when health is zero or less;
    public float nullFadeSpeed = 0.1F;
    public OnHit onHit;                         //On hit settings
}

[System.Serializable]
public class OnHit
{
    public float fadeSpeed = 0.1F;              //Alpha state fade speed;
    public float onHitAlpha = 1.0F;             //On hit alpha;
    public float duration = 1.0F;               //Duration of alpha state;
}
