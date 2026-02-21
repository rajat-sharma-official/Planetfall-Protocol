// NPC_Maerlon.ink
INCLUDE globals.ink

=== maerlon ===
-> maerlon_entry


=== maerlon_entry


// Branch between Script A and Script B
{ met_child:
    -> maerlon_B_hub
- else:
    -> maerlon_A_hub
}


// --------------------
// SCRIPT A (met_child == false)
// --------------------

=== maerlon_A_hub
Maerlon: "Have you seen a child come through here?"

+ [1) "No, not yet."] -> maerlon_A_no
+ [2) "Tell me who you're looking for."] -> maerlon_A_who
+ [3) "Why are you out here alone?"] -> maerlon_A_why
+ [4) "I'll keep an eye out."] -> maerlon_A_leave


=== maerlon_A_no
Maerlon: "Then I’ll keep searching. They can’t have gone far."
-> maerlon_A_hub


=== maerlon_A_who
Maerlon: "Small, quiet. Got separated near the path."
-> maerlon_A_hub


=== maerlon_A_why
Maerlon: "Because waiting is worse than walking."
-> maerlon_A_hub


=== maerlon_A_leave
Maerlon: "If you find them… come back."
-> END



// --------------------
// SCRIPT B (met_child == true)
// --------------------

=== maerlon_B_hub
Maerlon: "You found them? Are they safe?"

+ [1) "Yes. They’re safe."] -> maerlon_B_safe
+ [2) "They mentioned you by name."] -> maerlon_B_name
+ [3) "I can lead you to them."] -> maerlon_B_lead
+ [4) "Anything I should know?"] -> maerlon_B_info


=== maerlon_B_safe
Maerlon: "Thank you… truly."

// Give scrap only once
{ maerlon_gave_scrap == false:
    ~ maerlon_gave_scrap = true
    ~ Dialogue_scrap = Dialogue_scrap + 1
    Maerlon: "Take this scrap. It might keep you going out here."
- else:
    Maerlon: "I already gave you what I could spare."
}

-> maerlon_B_hub


=== maerlon_B_name
Maerlon: "Good… they remembered. That helps more than you think."
-> maerlon_B_hub


=== maerlon_B_lead
Maerlon: "I’ll follow your lead."
-> maerlon_B_hub


=== maerlon_B_info
Maerlon: "The path shifts when you stop paying attention. Keep moving."
-> maerlon_B_hub